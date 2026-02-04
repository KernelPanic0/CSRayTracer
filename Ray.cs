using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Ray
	{
		public Point3 origin { get; }
		public Vector3 direction { get; }
		
		public Ray(Point3 origin, Vector3 direction)
		{
			this.origin = origin;
			this.direction = direction;
		}

		public Point3 At(double t)
		{
			return (origin + t* direction);
		}
	}
}
