using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
	class Vector3
	{
		public double x { get; set; }
		public double y { get; set; }
		public double z { get; set; }

		public Vector3(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
        public Vector3(Point3 point)
        {
            this.x = point.x;
            this.y = point.y;
            this.z = point.z;
        }
        public double Length()
		{
			return Math.Sqrt(LengthSquared());
		}
		public double LengthSquared()
		{
			return x * x + y * y + z * z;
		}
		public static double Dot(Vector3 vector1, Vector3 vector2)
		{
			return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
		}
		public static Vector3 UnitVector(Vector3 vector)
		{
			return vector / vector.Length();
		}

		// Operator overloads to change functionality when doing math on 2 vectors
		// +
		public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
		}
		// -
		public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
		}
        public static Vector3 operator -(Vector3 vector, Point3 point)
        {
            return new Vector3(vector.x - point.x, vector.y - point.y, vector.z - point.z);
        }
        // *
        public static Vector3 operator *(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
		}
		public static Vector3 operator *(Vector3 vector, double t)
		{
			return new Vector3(vector.x * t, vector.y * t, vector.z * t);
		}
		public static Vector3 operator *(double t, Vector3 vector)
		{
			return new Vector3(vector.x * t, vector.y * t, vector.z * t);
		}
		// /
		public static Vector3 operator /(Vector3 vector, double t)
		{
			return new Vector3(vector.x / t, vector.y / t, vector.z / t);
		}
	}
	class Point3
	{
		public double x { get; set; }
		public double y { get; set; }
		public double z { get; set; }
		public Point3(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public Point3(Vector3 vector)
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}
        // Custom conversion method to cast from Vector3 to Colour3
        public static explicit operator Point3(Vector3 vector)
        {
            return new Point3(vector);
        }
        public static explicit operator Vector3(Point3 point)
        {
            return new Vector3(point);
        }

        public static Point3 operator +(Point3 point1, Point3 point2)
        {
            return new Point3(point1.x + point2.x, point1.y + point2.y, point1.z + point2.z);
        }
        public static Point3 operator +(Vector3 vector, Point3 point)
        {
            return new Point3(vector.x + point.x, vector.y + point.y, vector.z + point.z);
        }
        public static Point3 operator +(Point3 point, Vector3 vector)
        {
            return new Point3(vector.x + point.x, vector.y + point.y, vector.z + point.z);
        }
        // -
        public static Point3 operator -(Point3 point1, Point3 point2)
        {
            return new Point3(point1.x - point2.x, point1.y - point2.y, point1.z - point2.z);
        }
        public static Point3 operator -(Point3 point, Vector3 vector)
        {
            return new Point3(point.x - vector.x, point.y - vector.y, point.z - vector.z);
        }
        // *
        public static Point3 operator *(Point3 point1, Point3 point2)
        {
            return new Point3(point1.x * point2.x, point1.y * point2.y, point1.z * point2.z);
        }
        public static Point3 operator *(Point3 vector, double t)
        {
            return new Point3(vector.x * t, vector.y * t, vector.z * t);
        }
        public static Point3 operator *(double t, Point3 vector)
        {
            return new Point3(vector.x * t, vector.y * t, vector.z * t);
        }
        // /
        public static Point3 operator /(Point3 vector, double t)
        {
            return new Point3(vector.x / t, vector.y / t, vector.z / t);
        }
    }
	class Colour3
	{
		public double r { get; set; }
		public double g { get; set; }
		public double b { get; set; }
		public Colour3(double x, double y, double z)
		{
			this.r = x;
			this.g = y;
			this.b = z;
		}
		public Colour3(Vector3 vector)
		{
			this.r = vector.x;
			this.g = vector.y;
			this.b = vector.z;
		}
		// Custom conversion method to cast from Vector3 to Colour3
		public static explicit operator Colour3(Vector3 vector)
		{
			return new Colour3(vector);
		}

		// Operator overloads
		// +
		public static Colour3 operator +(Colour3 colour1, Colour3 colour2)
		{
			return new Colour3(colour1.r + colour2.r, colour1.g + colour2.g, colour1.b + colour2.b);
		}
		// -
		public static Colour3 operator -(Colour3 colour1, Colour3 colour2)
		{
			return new Colour3(colour1.r - colour2.r, colour1.g - colour2.g, colour1.b - colour2.b);
		}
		// *
		public static Colour3 operator *(Colour3 colour1, Colour3 colour2)
		{
			return new Colour3(colour1.r * colour2.r, colour1.g * colour2.g, colour1.b * colour2.b);
		}
		public static Colour3 operator *(Colour3 colour, double t)
		{
			return new Colour3(colour.r * t, colour.g * t, colour.b * t);
		}
		public static Colour3 operator *(double t, Colour3 colour)
		{
			return new Colour3(colour.r * t, colour.g * t, colour.b * t);
		}

	}
}
