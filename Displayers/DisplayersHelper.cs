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

        public static Vector3[] DrawFibonacciCapsule(Vector3 center, float radius, float height, float pointsPerUnit = 100f)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            if (pointsPerUnit <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerUnit));

            float cylinderHeight = Mathf.Max(0f, height - 2f * radius);
            int spherePoints = Mathf.RoundToInt(pointsPerUnit * radius);
            int cylinderPoints = Mathf.RoundToInt(pointsPerUnit * cylinderHeight);

            List<Vector3> points = new List<Vector3>(spherePoints * 2 + cylinderPoints);

            float offset = 2f / spherePoints;


            // Hemispheres
            for (int i = 0; i < spherePoints; i++)
            {
                float y = ((i * offset) - 1) + (offset / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = i * MathConstants.GOLDEN_ANGLE;

                float x = Mathf.Cos(phi) * r;
                float z = Mathf.Sin(phi) * r;

                Vector3 dir = new Vector3(x, y, z);

                if (y >= 0f) // top one
                {
                    Vector3 sphereCenter = center + Vector3.up * (cylinderHeight / 2f);
                    points.Add(sphereCenter + dir * radius);
                }
                else // bottom one
                {
                    Vector3 sphereCenter = center - Vector3.up * (cylinderHeight / 2f);
                    points.Add(sphereCenter + dir * radius);
                }
            }

            // Cyllinder
            for (int i = 0; i < cylinderPoints; i++)
            {
                float t = (float)i / cylinderPoints;
                float y = Mathf.Lerp(-cylinderHeight / 2f, cylinderHeight / 2f, t);

                float phi = i * MathConstants.GOLDEN_ANGLE;
                float x = Mathf.Cos(phi);
                float z = Mathf.Sin(phi);

                points.Add(center + new Vector3(x * radius, y, z * radius));
            }

            return points.ToArray();
        }

        public static Vector3[] DrawLatitudeLongitudeCapsule(Vector3 center, float radius, float height, float pointsPerUnit = 100f)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException(nameof(radius));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            if (pointsPerUnit <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerUnit));

            float cylinderHeight = Mathf.Max(0f, height - 2f * radius);

            int totalPoints = Mathf.RoundToInt(pointsPerUnit * (radius + cylinderHeight));
            int latSteps = Mathf.Max(4, Mathf.RoundToInt(Mathf.Sqrt(totalPoints / 2f)));
            int lonSteps = latSteps * 2;

            List<Vector3> points = new List<Vector3>();

            float lonStep = 2f * Mathf.PI / lonSteps;

            
            // Top hemisphere
            int hemiLatSteps = latSteps / 2;
            float latStep = (Mathf.PI / 2f) / hemiLatSteps;

            Vector3 topCenter = center + Vector3.up * (cylinderHeight / 2f);

            for (int i = 0; i <= hemiLatSteps; i++)
            {
                float a = i * latStep; // 0 to pi/2
                float sinA = Mathf.Sin(a);
                float cosA = Mathf.Cos(a);

                for (int j = 0; j < lonSteps; j++)
                {
                    float b = j * lonStep;

                    points.Add(
                        topCenter + new Vector3(
                            radius * sinA * Mathf.Cos(b),
                            radius * cosA,
                            radius * sinA * Mathf.Sin(b)
                        )
                    );
                }
            }

           

            // Cyllinder
            int cylinderSteps = Mathf.Max(1, Mathf.RoundToInt(pointsPerUnit * cylinderHeight / radius));
            float yStep = cylinderHeight / cylinderSteps;

            for (int i = 1; i < cylinderSteps; i++)
            {
                float y = -cylinderHeight / 2f + i * yStep;

                for (int j = 0; j < lonSteps; j++)
                {
                    float b = j * lonStep;

                    points.Add(
                        center + new Vector3(
                            radius * Mathf.Cos(b),
                            y,
                            radius * Mathf.Sin(b)
                        )
                    );
                }
            }

            // Bottom hemisphere
            Vector3 bottomCenter = center - Vector3.up * (cylinderHeight / 2f);

            for (int i = 0; i <= hemiLatSteps; i++)
            {
                float a = i * latStep; // 0 to pi/2
                float sinA = Mathf.Sin(a);
                float cosA = Mathf.Cos(a);

                for (int j = 0; j < lonSteps; j++)
                {
                    float b = j * lonStep;

                    points.Add(
                        bottomCenter + new Vector3(
                            radius * sinA * Mathf.Cos(b),
                            -radius * cosA,
                            radius * sinA * Mathf.Sin(b)
                        )
                    );
                }
            }

            return points.ToArray();
        }

    }
}
