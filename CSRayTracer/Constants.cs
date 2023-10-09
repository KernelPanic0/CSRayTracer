using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Constants
	{

		public static double infinity = double.PositiveInfinity;
		public static double pi = Math.PI;
		private static Random random = new Random();

		public double degreesToRadians(double degrees)
		{
			return degrees * pi / 180.0;
		}
	}
}
