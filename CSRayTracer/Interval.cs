using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Interval
	{
		public double min, max;

		public Interval(double min, double max)
		{
			this.min = min;
			this.max = max;
		}
		public bool Contains(double x) { return x >= min && x <= max;}
		public bool Surrounds(double x) { return x > min && x < max;}
		public double Clamp(double x)
		{
			if (x < min) return min;
			if (x > max) return max;
			return x;
		}
	}
}
