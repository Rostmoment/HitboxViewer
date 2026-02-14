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
        public static Vector3[] DrawFibonacciSphere(Vector3 center, float worldRadius, float pointsPerUnitArea = 0.1f)
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
        public static Vector3[] DrawLatitudeLongitudeSphere(Vector3 center, float worldRadius, float pointsPerRadius = 10f)
        {
            int pointsCount = Mathf.RoundToInt(worldRadius * pointsPerRadius);
            List<Vector3> points = new List<Vector3>();

            float latStep = Mathf.PI / pointsCount;      // step for latitude
            float lonStep = 2 * Mathf.PI / pointsCount; // step for longitude

            for (float a = 0; a <= Mathf.PI; a += latStep)
            {
                float sin = Mathf.Sin(a);
                float cos = Mathf.Cos(a);
                for (float b = 0; b <= 2 * Mathf.PI; b += lonStep)
                {
                    points.Add(center + new Vector3(
                        worldRadius * sin * Mathf.Cos(b),
                        worldRadius * sin * Mathf.Sin(b),
                        worldRadius * cos));
                }
            }

            BasePlugin.Logger.LogInfo(points.Count);
            return points.ToArray();
        }

    }
}
