using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class MathFunctions
    {
        public static Vector3 QuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }
    } 
}
