// Written by Yarik Panchenko at Cirencester College 2023.
// github.com/KernelPanic0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	internal class Program
	{
		static Colour3 RayColour(Ray ray)
		{
			Vector3 unitDirection = Vector3.UnitVector(ray.direction);
			double a = 0.5 * (unitDirection.y + 1.0);
			return (Colour3)((1.0-a)*new Colour3(1.0, 1.0, 1.0)+a*new Colour3(0.5, 0.7, 1.0));
		}
		static void Main(string[] args)
		{
			double aspectRatio = 16.0 / 9.0;
			int imageWidth = 720; // Pixels
			double imageHeight = (imageWidth / aspectRatio);

			//Camera 
			double focalLength = 1.0;
			double viewportHeight = 2.0;
			double viewportWidth = viewportHeight * ((double)imageWidth/imageHeight);
			Point3 cameraCenter = new Point3(0, 0, 0);

			Vector3 viewportHorizontal = new Vector3(viewportWidth, 0, 0);
			Vector3 viewportVertical = new Vector3(0, -viewportHeight, 0); // Currently the Y axis is inverted because of conflicts with image coordinates

			Vector3 pixelDeltaHorizontal = viewportHorizontal / imageWidth;
			Vector3 pixelDeltaVertical = viewportVertical / imageHeight;

			//Upper left pixel
			Vector3 viewportUpperLeft = cameraCenter - new Vector3(0, 0, focalLength) - viewportHorizontal/2 - viewportVertical/2;
			Vector3 pixel00Loc = viewportUpperLeft + 0.5 * (pixelDeltaHorizontal + pixelDeltaVertical);

			WriteMetadata(imageWidth, imageHeight);
			Console.WriteLine("Wrote metadata");
			string resultBuffer = ""; // For storing result

			StreamWriter writer = new StreamWriter("./render.ppm", true);
			for (int j = 0; j < imageHeight; j++)
			{
				resultBuffer = "";
				for (int i = 0; i < imageWidth; i++)
				{
					Vector3 pixelCenter = pixel00Loc + (i*pixelDeltaHorizontal) + (j*pixelDeltaVertical);
					Vector3 rayDirection = pixelCenter - cameraCenter;
					Ray ray = new Ray(cameraCenter, rayDirection);

					Colour3 pixelColour = RayColour(ray);



					// r g and b will go from 1 to 0

                    double r = 255.999 * pixelColour.r;
					double g = 255.999 * pixelColour.g;
					double b = 255.999 * pixelColour.b;

					int ir = (int)(r);
					int ig = (int)(g);
					int ib = (int)(b);
					resultBuffer += $"{ir} {ig} {ib}\n";
				}
				writer.Write(resultBuffer);
			}
			Console.ReadLine();
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
