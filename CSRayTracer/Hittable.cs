﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRayTracer
{
    class Hittable
    {
        
        public virtual bool Hit(Ray ray, Interval rayT, ref HitRecord hit_record)
        {
            return false;
        }
    }

    class HittableList: Hittable
    {
        List<Hittable> objects = new List<Hittable>();
        HittableList(Hittable objectToAdd)
        {
            objects.Add(objectToAdd);
        }

		public HittableList() { }

        public void Add(Hittable objectToAdd)
        {
            objects.Add(objectToAdd);
		}

        public override bool Hit(Ray ray, Interval rayT, ref HitRecord hitRecord)
        {
            HitRecord tempRecord = new HitRecord();
            bool hitAnything = false;
            double closestSoFar = rayT.max;

            foreach (Hittable obj in objects)
            {
                if (obj.Hit(ray, new Interval(rayT.min, closestSoFar), ref tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.t;
                    hitRecord = tempRecord;
                }
            } 
            return hitAnything;
        }
    }

    class HitRecord
    {
        public Point3 point { get; set; }
        public Vector3 normal { get; set; }
        public Material material { get; set; }
        public double t { get; set; }
        public bool frontFace { get; set; }

        public void SetFaceNormal(Ray ray, Vector3 outwardNormal) 
        {
            // The parameter outward should be of unit length.
            frontFace = Vector3.Dot(ray.direction, outwardNormal) < 0;
            normal = frontFace ? outwardNormal : -outwardNormal;
        }
    }
}
