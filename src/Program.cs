// Written by Yarik Panchenko at Cirencester College 2023.
// github.com/KernelPanic0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace CSRayTracer
{
	internal class Program
	{

		static void Main(string[] args)
		{
			//Materials

			Lambertian mSurface = new Lambertian(new Colour3(1, 1, 1));
			Lambertian matL = new Metal(new Colour3(1, 0.188, 0.188));
			Lambertian matR = new Lambertian(new Colour3(0.023, 0.360, 0));
			DiffuseLight diffuseLight = new DiffuseLight(new Colour3(0.8125, 1.09375, 3.671875));


			//World 
			HittableList world = new HittableList();
			Sphere lightSource = new Sphere(new Point3(0, 2.8, -2), 1, diffuseLight);

			//Cornell Box
			Sphere floor = new Sphere(new Point3(0, -50002, -5), 50000, mSurface);
			Sphere rightWall = new Sphere(new Point3(50002, 0, -5), 50000, matR);
			Sphere leftWall = new Sphere(new Point3(-50002, 0, -5), 50000, matL);
			Sphere ceiling = new Sphere(new Point3(0, 50002, -5), 50000, mSurface);
			Sphere backWall = new Sphere(new Point3(0, 0, -50003), 50000, mSurface);
			Sphere frontWall = new Sphere(new Point3(0, 0, 50003), 50000, mSurface);

			//Scene

			Lambertian lSphere = new Lambertian(new Colour3(1, 0.549, 0));
			Sphere leftSphere = new Sphere(new Point3(-1, -1.5, -2.5), .5, lSphere);

			Metal mSphere = new Metal(new Colour3(0.2705, 0.356, 1), 0.4);
			Sphere middleSphere = new Sphere(new Point3(0, -1.5, -2), .5, mSphere);

			Metal rSphere = new Metal(new Colour3(0.8, 0.8, 0.8));
			Sphere rightSphere = new Sphere(new Point3(1, -1.5, -2.5), .5, rSphere);


			world.Add(lightSource);
			world.Add(rightWall);
			world.Add(leftWall);
			world.Add(ceiling);
			world.Add(floor);
			world.Add(backWall);
			world.Add(frontWall);
			world.Add(leftSphere);
			world.Add(middleSphere);
			world.Add(rightSphere);

			const int imageWidth = 400;
			Camera camera = new Camera();
			UI ui = new UI(imageWidth);

			ui.StartRenderTask();

			camera.imageWidth = imageWidth;
			camera.samplesPerPixel = 10;
			camera.maxDepth = 5;
			camera.background = new Colour3(0, 0, 0);
			camera.Render(world, ui);
		}
	}
}
