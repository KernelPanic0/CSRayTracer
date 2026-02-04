using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Material
	{
		public virtual bool Scatter(Ray rayIn, HitRecord hitRecord, ref Colour3 attenuation, ref Ray scattered)
		{
			return true;
		}

		public virtual Colour3 Emitted(double u, double v, Point3 point)
		{
			return new Colour3(0, 0, 0);
		}
	}
	
	class Lambertian : Material
	{
		private Colour3 albedo;
		public Lambertian(Colour3 albedo)
		{
			this.albedo = albedo;
		}

		public override bool Scatter(Ray rayIn, HitRecord hitRecord, ref Colour3 attenuation, ref Ray scattered)
		{
			Vector3 scatterDirection = hitRecord.normal + Vector3.RandomUnitVector();

			if (scatterDirection.NearZero())
				scatterDirection = hitRecord.normal;

			scattered = new Ray(hitRecord.point, scatterDirection);
			attenuation = albedo;
			return true;

		}
	}

	class Metal : Lambertian
	{
		private Colour3 albedo;
		private double fuzz;
		public Metal(Colour3 albedo, double fuzz) : base(albedo)
		{ 
			this.albedo = albedo;
			this.fuzz = fuzz;
		}

		public Metal(Colour3 albedo) : base(albedo)
		{
			this.albedo = albedo;
			this.fuzz = 0;
		}

		public override bool Scatter(Ray rayIn, HitRecord hitRecord, ref Colour3 attenuation, ref Ray scattered)
		{
			Vector3 reflected = Vector3.Reflect(Vector3.UnitVector(rayIn.direction), hitRecord.normal);
			scattered = new Ray(hitRecord.point, reflected + fuzz*Vector3.RandomUnitVector());
			attenuation = albedo;
			return Vector3.Dot(scattered.direction, hitRecord.normal) > 0;
		}
	}

	class DiffuseLight : Material
	{
		private Colour3 emit;
		public DiffuseLight(Colour3 emit)
		{
			this.emit = emit;
		}

		public override bool Scatter(Ray rayIn, HitRecord hitRecord, ref Colour3 attenuation, ref Ray scattered)
		{
			return false;
		}
		public override Colour3 Emitted(double u, double v, Point3 point)
		{
			return emit;
		}
	}
}
