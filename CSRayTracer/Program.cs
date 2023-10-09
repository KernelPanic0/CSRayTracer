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
			//World 
			HittableList world = new HittableList();
			Sphere sphere = new Sphere(new Point3(0, 0.5, -4), 1);
			Sphere surface = new Sphere(new Point3(0, -5005, -5), 5000);
			world.Add(sphere);
			world.Add(surface);

			Camera camera = new Camera();

			camera.imageWidth = 400;
			camera.Render(world);
		}
	}
}
