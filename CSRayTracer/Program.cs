﻿// Written by Yarik Panchenko at Cirencester College 2023.
// github.com/KernelPanic0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	internal class Program
	{
		static Point3 sphere = new Point3 (1, 0, -1);
		static double HitSphere(Point3 centre, double radius, Ray ray)
		{
			Vector3 oc = (Vector3)(ray.origin - centre);
			double a = Vector3.Dot(ray.direction, ray.direction);
			double b = 2.0 * Vector3.Dot(oc, ray.direction);
			double c = Vector3.Dot(oc, oc) - radius * radius;
			double discriminant = b * b - 4 * a * c; 
			if (discriminant < 0) // The ray does NOT intersect with the sphere.
			{
				return -1.0; // return -1.0, which would be BEHIND the ray, indicating no ray-sphere intersection
			} else {
				return (-b - Math.Sqrt(discriminant)) / (2.0 * a);
			}

		}
		static Colour3 RayColour(Ray ray)
		{
			double t = HitSphere(sphere, 0.5, ray);
			if (t > 0.0)
			{
				Vector3 N = Vector3.UnitVector((Vector3)ray.At(t) - new Vector3(0, 0, -1));
				return 0.5 * new Colour3(N.x + 1, N.y + 1, N.z + 1);
			}

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
			Vector3 viewportUpperLeft = (Vector3)(cameraCenter - new Vector3(0, 0, focalLength) - viewportHorizontal/2 - viewportVertical/2);
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
			writer.Close();
			Console.WriteLine("Finished");
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
