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
			Metal glass = new Metal(new Colour3(1, 1, 1), 0.05);
			// DiffuseLight diffuseLight = new DiffuseLight(new Colour3(1, 1, 1));
			DiffuseLight diffuseLight2 = new DiffuseLight(new Colour3(0.258, 0.529, 0.967), 3);
			DiffuseLight diffuseLight3 = new DiffuseLight(new Colour3(0.890, 0.835, 0.117), 3);
			DiffuseLight diffuseLight4 = new DiffuseLight(new Colour3(0.490, 0.980, 0.117), 3);
			DiffuseLight diffuseLight5 = new DiffuseLight(new Colour3(0.117, 0.890, 0.890), 3);
			DiffuseLight diffuseLight6 = new DiffuseLight(new Colour3(0.117, 0.270, 0.890), 3);
			DiffuseLight diffuseLight7 = new DiffuseLight(new Colour3(0.321, 0.117, 0.890), 3);
			DiffuseLight diffuseLight8 = new DiffuseLight(new Colour3(0.890, 0.117, 0.890), 3);
			DiffuseLight diffuseLight = new DiffuseLight(new Colour3(0.890, 0.117, 0.282), 3);


			//World 
			HittableList world = new HittableList();
			// Sphere lightSource = new Sphere(new Point3(0, 2.8, -2), 1, diffuseLight);
			Sphere lightSource1 = new Sphere(new Point3(-1.5, 1, -3), 0.3, diffuseLight2);
			Sphere lightSource2 = new Sphere(new Point3(-1.125, 1, -3), 0.3, diffuseLight3);
			Sphere lightSource3 = new Sphere(new Point3(-0.75, 1, -3), 0.3, diffuseLight4);
			Sphere lightSource4 = new Sphere(new Point3(-0.375, 1, -3), 0.3, diffuseLight5);
			Sphere lightSource5 = new Sphere(new Point3(0, 1, -3), 0.3, diffuseLight6);
			Sphere lightSource6 = new Sphere(new Point3(0.375, 1, -3), 0.3, diffuseLight7);
			Sphere lightSource7 = new Sphere(new Point3(0.75, 1, -3), 0.3, diffuseLight8);
			Sphere lightSource8 = new Sphere(new Point3(1.25, 1, -3), 0.3, diffuseLight);

			//Cornell Box
			Sphere floor = new Sphere(new Point3(0, -50002, -5), 50000, mSurface);
			Sphere rightWall = new Sphere(new Point3(50002, 0, -5), 50000, matR);
			Sphere leftWall = new Sphere(new Point3(-50002, 0, -5), 50000, matL);
			Sphere ceiling = new Sphere(new Point3(0, 50002, -5), 50000, mSurface);
			Sphere backWall = new Sphere(new Point3(0, 0, -50003), 50000, glass);
			Sphere frontWall = new Sphere(new Point3(0, 0, 50003), 50000, glass);

			//Scene
			Lambertian lSphere = new Lambertian(new Colour3(1, 0.549, 0));
			Sphere leftSphere = new Sphere(new Point3(-1, -1.5, -2.5), .5, lSphere);

			Metal mSphere = new Metal(new Colour3(0.2705, 0.356, 1), 0.4);
			Sphere middleSphere = new Sphere(new Point3(0, -1.5, -2), .5, mSphere);

			Metal rSphere = new Metal(new Colour3(0.8, 0.8, 0.8));
			Sphere rightSphere = new Sphere(new Point3(1, -1.5, -2.5), .5, rSphere);

			// world.Add(lightSource);
			world.Add(lightSource2);
			world.Add(lightSource3);
			world.Add(lightSource4);
			world.Add(lightSource5);
			world.Add(lightSource6);
			world.Add(lightSource7);
			world.Add(lightSource8);
			// world.Add(lightSource3);
			world.Add(rightWall);
			world.Add(leftWall);
			world.Add(ceiling);
			world.Add(floor);
			world.Add(backWall);
			world.Add(frontWall);
			world.Add(leftSphere);
			world.Add(middleSphere);
			world.Add(rightSphere);

			const int imageWidth = 1920;

			Lock renderLock = new Lock();
			UI ui = new UI(imageWidth);
			Camera camera = new Camera(world, ui, renderLock);

			camera.imageWidth = imageWidth;
			camera.samplesPerPixel = 600;
			camera.maxDepth = 80;
			camera.background = new Colour3(0, 0, 0);
			camera.StartRenderTask();

			while (!Raylib.WindowShouldClose())
			{
				ui.Draw(renderLock);
			}
		}
	}
}
