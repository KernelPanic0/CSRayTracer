using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace CSRayTracer
{
	class Camera
	{
		public double aspectRatio = 16.0 / 9.0;
		public int imageWidth = 700; // Pixels
		public int samplesPerPixel = 100;
		public int maxDepth = 40;
		public Colour3 background;
		private double imageHeight;
		private Point3 centre;
		private Point3 pixel00Loc;
		private Vector3 pixelDeltaHorizontal;
		private Vector3 pixelDeltaVertical;
		// private DateTime lastRenderEpoch = DateTime.UtcNow;
		private int pixelCount;
		private int rayCount;
		private Hittable world;
		private UI ui;
		private Lock renderLock;
		private Random random = new Random();

		public void StartRenderTask()
		{
			Task renderTask = new Task(() =>
			{
				Render();
			});

			renderTask.Start();
		}

		private void CalculatePPS()
		{
			Task.Run(async () =>
			{
				while (!Raylib.WindowShouldClose())
				{
					await Task.Delay(3000);
					ui.pixelsPerSecond = pixelCount / 3;
					ui.raysPerSecond = rayCount / 3;
					Interlocked.Exchange(ref pixelCount, 0);
					Interlocked.Exchange(ref rayCount, 0);
				}
			});
		}

		public Camera(Hittable world, UI ui, Lock renderLock)
		{
			this.world = world;
			this.ui = ui;
			this.renderLock = renderLock;
		}

		private void Render()
		{
			InitialiseProperties();

			int[] rowList = Enumerable.Range(0, (int)imageHeight).ToArray();
			new Random().Shuffle(rowList);

			ConcurrentQueue<int> rowQueue = new ConcurrentQueue<int>();

			foreach (int row in rowList)
			{
				rowQueue.Enqueue(row);
			}

			Action traceRow = () =>
			{
				int row = 0;
				while (rowQueue.TryDequeue(out row))
				{
					Raylib_cs.Color[] rowPixels = new Raylib_cs.Color[imageWidth];
					for (int x = 0; x < imageWidth; x++)
					{
						int y = row;

						Colour3 pixelColour = new Colour3(0, 0, 0);
						for (int sample = 0; sample < samplesPerPixel; sample++)
						{
							Ray ray = GetRay(x, y);
							Interlocked.Increment(ref rayCount);
							pixelColour += RayColour(ray, maxDepth, this.world);
						}
						double r = ComputeColour(pixelColour.r, samplesPerPixel);
						double g = ComputeColour(pixelColour.g, samplesPerPixel);
						double b = ComputeColour(pixelColour.b, samplesPerPixel);

						int ir = (int)r;
						int ig = (int)g;
						int ib = (int)b;

						rowPixels[x] = new Raylib_cs.Color(ir, ig, ib);
						Interlocked.Increment(ref pixelCount);
					}

					this.ui.AppendRow(rowPixels, imageWidth * row);
				}
			};

			int workerCount = Environment.ProcessorCount;
			Task[] workers = new Task[workerCount];

			for (int i = 0; i < workerCount; i++)
			{
				workers[i] = Task.Run(traceRow);
			}

			CalculatePPS();
			Task.WaitAll(workers);

			Console.WriteLine("Finished");
			Console.ReadLine();
		}
		private double ComputeColour(double colour, int samplesPerPixel)
		{
			double scale = 1.0 / samplesPerPixel;
			colour *= scale;
			colour = LinearToGamma(colour);
			Interval intensity = new Interval(0.000, 0.999);
			return 255.999 * intensity.Clamp(colour);

		}
		private Ray GetRay(int i, int j)
		{
			// Get a randomly sampled camera ray for the pixel at location i,j.

			Vector3 pixelCentre = (Vector3)pixel00Loc + (i * pixelDeltaHorizontal) + (j * pixelDeltaVertical);
			Vector3 pixelSample = pixelCentre + pixelSampleSquare();

			Point3 rayOrigin = centre;
			Vector3 rayDirection = pixelSample - rayOrigin;

			return new Ray(rayOrigin, rayDirection);
		}
		private Vector3 pixelSampleSquare() // Returns a random point in the square surroinding a pixel at the origin.
		{
			double pX = -0.5 + random.NextDouble();
			double pY = -0.5 + random.NextDouble();

			return (pX * pixelDeltaHorizontal) + (pY * pixelDeltaVertical);
		}
		private void InitialiseProperties()
		{
			imageHeight = imageWidth / aspectRatio;
			imageHeight = (imageHeight < 1) ? 1 : imageHeight;

			centre = new Point3(0, 0, 0);

			//Camera 
			double focalLength = 1.0;
			double viewportHeight = 2.0;
			double viewportWidth = viewportHeight * ((double)imageWidth / imageHeight);
			Point3 cameraCenter = new Point3(0, 0, 0);

			Vector3 viewportHorizontal = new Vector3(viewportWidth, 0, 0);
			Vector3 viewportVertical = new Vector3(0, -viewportHeight, 0); // Currently the Y axis is inverted because of conflicts with image coordinates

			pixelDeltaHorizontal = viewportHorizontal / imageWidth;
			pixelDeltaVertical = viewportVertical / imageHeight;

			//Upper left pixel
			Vector3 viewportTopLeft = (Vector3)(cameraCenter - new Vector3(0, 0, focalLength) - viewportHorizontal / 2 - viewportVertical / 2);
			pixel00Loc = (Point3)viewportTopLeft + 0.5 * (pixelDeltaHorizontal + pixelDeltaVertical);
		}
		private double LinearToGamma(double colour)
		{
			return Math.Sqrt(colour);
		}
		private Colour3 RayColour(Ray ray, int depth, Hittable world)
		{
			if (depth <= 0)
				return new Colour3(0, 0, 0);

			HitRecord hitRecord = new HitRecord();
			Interval rayTInterval = new Interval(0.001, Constants.infinity);
			if (!world.Hit(ray, rayTInterval, ref hitRecord))
			{
				return background;
			}
			Ray scattered = new Ray(new Point3(0, 0, 0), new Vector3(0, 0, 0));
			Colour3 attenuation = new Colour3(0, 0, 0);
			Colour3 colourFromEmission = hitRecord.material.Emitted(0, 0, hitRecord.point);

			if (!hitRecord.material.Scatter(ray, hitRecord, ref attenuation, ref scattered))
				return colourFromEmission;
			Colour3 colourFromScatter = attenuation * RayColour(scattered, depth - 1, world);

			return colourFromEmission + colourFromScatter;

			/*			return attenuation * RayColour(scattered, depth-1, world);
						return new Colour3(0, 0, 0);
						Vector3 direction = hitRecord.normal + Vector3.RandomUnitVector();
						return 0.5 * RayColour(new Ray(hitRecord.point, direction), depth-1, world);

						// Render background
						Vector3 unitDirection = Vector3.UnitVector(ray.direction);
						double a = 0.5 * (unitDirection.y + 1.0);
						return (1.0 - a) * new Colour3(1.0, 1.0, 1.0) + a * new Colour3(0.5, 0.7, 1.0); // Linearly blend white and blue depending on y coordinate. Aka a Lerp
			*/
		}
	}
}
