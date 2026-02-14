using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace HitboxViewer.Displayers
{
    public static class DisplayersHelper
    {
        public static Vector3[] DrawFibonacciSphere(Vector3 center, float worldRadius, float pointsPerUnitArea = 1)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException("Radius should be positive");


            int pointsCount = Mathf.RoundToInt(pointsPerUnitArea * 4f * Mathf.PI * worldRadius * worldRadius);
            float offset = 2f / pointsCount;

            Vector3[] points = new Vector3[pointsCount];

            for (int i = 0; i < pointsCount; i++)
            {
                float y = ((i * offset) - 1) + (offset / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = i * MathConstants.GOLDEN_ANGLE;

                float x = Mathf.Cos(phi) * r;
                float z = Mathf.Sin(phi) * r;

                points[i] = center + new Vector3(x, y, z) * worldRadius;

            }

            return points;
        }
    }
}
