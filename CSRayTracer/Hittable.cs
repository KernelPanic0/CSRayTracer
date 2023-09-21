using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
    class Hittable
    {
        
        public virtual bool hit(Ray ray, double ray_tmin, double ray_tmax, HitRecord hit_record)
        {
            return false;
        }
    }

    class HitRecord
    {
        public Point3 point { get; }
        public Vector3 normal { get; }
        public double t { get; }
    }
}
