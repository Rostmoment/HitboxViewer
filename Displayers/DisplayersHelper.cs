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
        public static Vector3[] DrawFibonacciSphere(Vector3 center, float worldRadius, float pointsPerRadius = 100f)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(worldRadius), "Radius should be positive");

            if (pointsPerRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerRadius), "Point per radius should be positive");


            int pointsCount = Mathf.RoundToInt(pointsPerRadius * worldRadius);
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

        public static Vector3[] DrawLatitudeLongitudeSphere(Vector3 center, float worldRadius, float pointsPerRadius = 100f)
        {
            int totalPoints = Mathf.RoundToInt(pointsPerRadius * worldRadius);

            int latSteps = Mathf.Max(4, Mathf.RoundToInt(Mathf.Sqrt(totalPoints / 2f)));
            int lonSteps = latSteps * 2;

            Vector3[] points = new Vector3[(latSteps + 1) * lonSteps];

            float latStep = Mathf.PI / latSteps;
            float lonStep = 2f * Mathf.PI / lonSteps;

            int index = 0;

            for (int i = 0; i <= latSteps; i++)
            {
                float a = i * latStep;
                float sinA = Mathf.Sin(a);
                float cosA = Mathf.Cos(a);

                for (int j = 0; j < lonSteps; j++)
                {
                    float b = j * lonStep;
                    points[index++] = center + new Vector3(
                        worldRadius * sinA * Mathf.Cos(b),
                        worldRadius * sinA * Mathf.Sin(b),
                        worldRadius * cosA);
                }
            }

            return points;
        }

    }
}
