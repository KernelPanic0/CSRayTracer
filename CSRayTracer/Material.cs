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
		public Metal(Colour3 albedo) : base(albedo)
		{ 
			this.albedo = albedo;
		}

		public override bool Scatter(Ray rayIn, HitRecord hitRecord, ref Colour3 attenuation, ref Ray scattered)
		{
			Vector3 reflected = Vector3.Reflect(Vector3.UnitVector(rayIn.direction), hitRecord.normal);
			scattered = new Ray(hitRecord.point, reflected);
			attenuation = albedo;
			return true;
		}
	}
}
