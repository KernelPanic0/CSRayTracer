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

		static void Main(string[] args)
		{
			//Materials
			Lambertian matGround = new Lambertian(new Colour3(0.8, 0.8, 0.0));
			Metal matL = new Metal(new Colour3(0.8, 0.8, 0.8));
			Lambertian matR = new Lambertian(new Colour3(0.7, 0.3, 0.3));

			//World 
			HittableList world = new HittableList();
			Sphere sphere = new Sphere(new Point3(1, 0, -2), 1, matR);
			Sphere reflectiveSphere = new Sphere(new Point3(-1, 0, -2), 1, matL);

			Sphere surface = new Sphere(new Point3(0, -5001, -5), 5000, matGround);
			world.Add(sphere);
			world.Add(reflectiveSphere);
			world.Add(surface);

			Camera camera = new Camera();

			camera.imageWidth = 400;
			camera.Render(world);
		}
	}
}
