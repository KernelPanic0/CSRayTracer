// Written by Yarik Panchenko at Cirencester College 2023.
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

        static double HitSphere(Point3 centre, double radius, Ray ray) // Calculate whether a ray hit a sphere
		{
			Vector3 oc = (Vector3)(ray.origin - centre); // Vector to the centre of the sphere

			// Coeficients used for the quadratic equation in order to find the intersection points of the ray.
			double a = ray.direction.LengthSquared(); // Any point P that satisfies the equation (x - Cx)^2 + (y - Cy)^2 + (z - Cz)^2 = r^2  
            double half_b = Vector3.Dot(oc, ray.direction); // 
			double c = oc.LengthSquared() - radius * radius;
			// Calculate discriminant to determine whether thxere are real solutions or not (ie if the sphere was intersected in the first place)
			double discriminant = half_b*half_b-a*c; 
			if (discriminant < 0) // The ray does NOT intersect with the sphere.
			{
				return -1.0; // return -1.0, which would be BEHIND the ray, indicating no ray-sphere intersection
			} else {
				// Calculate the closest intersection point along the ray
				return (-half_b - Math.Sqrt(discriminant)) / a;
			}

		}
		static Colour3 RayColour(Ray ray, Hittable world)
		{
			/*double t = HitSphere(sphere, 0.5, ray);
			if (t > 0.0) // If t is in front of the ray (ie if it hit something)
			{
				Vector3 N = Vector3.UnitVector((Vector3)ray.At(t) - new Vector3(1, 0, -1));
				return 0.5 * new Colour3(N.x + 1, N.y + 1, N.z + 1); // Normalises the range from -1 - 1 to 0 - 1
            }*/
			HitRecord hitRecord = new HitRecord();
			if(world.Hit(ray, 0, 30000.00, ref hitRecord))
			{
				return 0.5 * (Colour3)(hitRecord.normal + new Colour3(1, 1, 1));
			}
			// Render background
			Vector3 unitDirection = Vector3.UnitVector(ray.direction);
			double a = 0.5 * (unitDirection.y + 1.0);
			return (Colour3)((1.0-a)*new Colour3(1.0, 1.0, 1.0)+a*new Colour3(0.5, 0.7, 1.0)); // Linearly blend white and blue depending on y coordinate. Aka a Lerp
        }
		static void Main(string[] args)
		{
			double aspectRatio = 16.0 / 9.0;
			int imageWidth = 700; // Pixels
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


			//World 
			HittableList world = new HittableList();
			Sphere sphere = new Sphere(new Point3(0, 0, -5), 1);
			Sphere surface = new Sphere(new Point3(0, -5002, -5), 5000);
			world.Add(surface);
			world.Add(sphere);


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

					Colour3 pixelColour = RayColour(ray, world);


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
