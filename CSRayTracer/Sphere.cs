using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
    class Sphere: Hittable
    {
        private Point3 centre;
        private double radius;
        public override bool Hit(Ray ray, Interval rayT, ref HitRecord hitRecord)
        {
            Vector3 oc = (Vector3)(ray.origin - centre); // Vector to the centre of the sphere

            // Coeficients used for the quadratic equation in order to find the intersection points of the ray.
            double a = ray.direction.LengthSquared(); 
            double half_b = Vector3.Dot(oc, ray.direction); //
            double c = oc.LengthSquared() - radius * radius;
            // Calculate discriminant to determine whether there are real solutions or not (ie if the sphere was intersected in the first place)
            double discriminant = half_b * half_b - a * c;
            if (discriminant < 0) return false; // The ray does NOT intersect with the sphere.
            double sqrtd = Math.Sqrt(discriminant);

            // Find nearest root thats within rayTMin and rayTMax
            double root = (-half_b - sqrtd) / a;
            if (!rayT.Surrounds(root))
            {
                root = (-half_b + sqrtd) / a;
                if(!rayT.Surrounds(root))
                {
                    return false;
                }
            }

            hitRecord.t = root;
            hitRecord.point = ray.At(hitRecord.t);
            Vector3 outwardNormal = (Vector3)(hitRecord.point - centre) / radius;
            hitRecord.SetFaceNormal(ray, outwardNormal);
            return true;
        }
        public Sphere(Point3 centre, double radius)
        {
            this.centre = centre;
            this.radius = radius;
        }
    }
}
