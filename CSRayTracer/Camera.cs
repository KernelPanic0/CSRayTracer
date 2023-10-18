using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Camera
	{
		public double aspectRatio = 16.0 / 9.0;
		public int imageWidth = 700; // Pixels
		public int samplesPerPixel = 100;
		public int maxDepth = 10;
		private double imageHeight;
		private Point3 centre;
		private Point3 pixel00Loc;
		private Vector3 pixelDeltaHorizontal;
		private Vector3 pixelDeltaVertical;
		private Random random = new Random();

		public void Render(Hittable world)
		{
			Initialise();
			WriteMetadata(imageWidth, imageHeight);
            Console.WriteLine("Wrote metadata.");

			string resultBuffer = ""; // For storing result

			StreamWriter writer = new StreamWriter("./render.ppm", true);
			for (int j = 0; j < imageHeight; j++)
			{
				for (int i = 0; i < imageWidth; i++)
				{
					Colour3 pixelColour = new Colour3(0, 0, 0);
					for(int sample = 0; sample < samplesPerPixel; sample++)
					{
						Ray ray = GetRay(i, j);
						pixelColour += RayColour(ray, maxDepth, world);
					}
					double r = ComputeColour(pixelColour.r, samplesPerPixel);
					double g = ComputeColour(pixelColour.g, samplesPerPixel);
					double b = ComputeColour(pixelColour.b, samplesPerPixel);



					int ir = (int)(r);
					int ig = (int)(g);
					int ib = (int)(b);
					resultBuffer += $"{ir} {ig} {ib}\n";
				}
			}
			writer.Write(resultBuffer);
			writer.Close();
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
		private void Initialise()
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
			if (world.Hit(ray, rayTInterval, ref hitRecord))
			{
				Ray scattered = new Ray(new Point3(0, 0, 0), new Vector3(0, 0, 0));
				Colour3 attenuation = new Colour3(0, 0, 0);
				if (hitRecord.material.Scatter(ray, hitRecord, ref attenuation, ref scattered))
					return attenuation * RayColour(scattered, depth-1, world);
				return new Colour3(0, 0, 0);
				Vector3 direction = hitRecord.normal + Vector3.RandomUnitVector();
				return 0.5 * RayColour(new Ray(hitRecord.point, direction), depth-1, world);
			}
			// Render background
			Vector3 unitDirection = Vector3.UnitVector(ray.direction);
			double a = 0.5 * (unitDirection.y + 1.0);
			return (Colour3)((1.0 - a) * new Colour3(1.0, 1.0, 1.0) + a * new Colour3(0.5, 0.7, 1.0)); // Linearly blend white and blue depending on y coordinate. Aka a Lerp
		}
		static void WriteMetadata(int width, double height)
		{
			using (StreamWriter writer = new StreamWriter("./render.ppm"))
			{
				writer.Write($"P3\n{width.ToString()} {height.ToString()}\n255\n");
				/// Prefixes the file with the following necesarry metadata for the file format to understand the image 
				/// P3      <- Declares that colours are in ASCII
				/// 720 405 <- Declares resoltuoin
				/// 255     <- Declares colour range
			}
		}
	}
}
