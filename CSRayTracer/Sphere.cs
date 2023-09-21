using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
    internal class Sphere: Hittable
    {
        private Point3 radius;
        private double raidus;
        bool hit(Ray ray, double ray)
        {
            Vector3 oc = (Vector3)(ray.origin - centre); // Vector to the centre of the sphere

            // Coeficients used for the quadratic equation in order to find the intersection points of the ray.
            double a = Vector3.Dot(ray.direction, ray.direction); // Any point P that satisfies the equation (x - Cx)^2 + (y - Cy)^2 + (z - Cz)^2 = r^2  
            double b = 2.0 * Vector3.Dot(oc, ray.direction); // 
            double c = Vector3.Dot(oc, oc) - radius * radius;
            // Calculate discriminant to determine whether there are real solutions or not (ie if the sphere was intersected in the first place)
            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0) // The ray does NOT intersect with the sphere.
            {
                return -1.0; // return -1.0, which would be BEHIND the ray, indicating no ray-sphere intersection
            }
            else
            {
                // Calculate the closest intersection point along the ray
                return (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            }
        }
    }
}
