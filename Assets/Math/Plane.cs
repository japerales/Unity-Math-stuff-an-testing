using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathLibrary
{
    public class Plane
    {
        Vector3 point1, point2, point3;

        public Plane(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.point1 = p1;
            this.point2 = p2;
            this.point3 = p3;
        }

        public Vector3 GetPointInPlane(float x, float y)
        {
            //A + vs + ut
            return point1 + point2 * x + point3 * y;
        }
    }
}