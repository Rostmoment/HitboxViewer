using HitboxViewer.Configs;
using HitboxViewer.Enums;
using HitboxViewer.Extensions;
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
        #region spheres
        public static Vector3[] DrawFibonacciSphere(Vector3 center, float worldRadius, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
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

        public static Vector3[] DrawLatitudeLongitudeSphere(Vector3 center, float worldRadius, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(worldRadius), "Radius should be positive");

            if (pointsPerRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerRadius), "Point per radius should be positive");

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

        public static Vector3[] DrawTwoAxisSphere(Vector3 center, float worldRadius, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            Vector3[] xz = DrawCircle(center, worldRadius, Enums.Plane.XZ, pointsPerRadius);
            Vector3[] xy = DrawCircle(center, worldRadius, Enums.Plane.XY, pointsPerRadius);

            return ArrayExtensions.Merge(xz, xy);
        }

        public static Vector3[] DrawThreeAxisSphere(Vector3 center, float worldRadius, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            Vector3[] xz = DrawCircle(center, worldRadius, Enums.Plane.XZ, pointsPerRadius);
            Vector3[] yz = DrawCircle(center, worldRadius, Enums.Plane.YZ, pointsPerRadius);

            Vector3[] xyFirst = DrawCircleQuarter(center, worldRadius, Quadrant.First, Enums.Plane.XY, pointsPerRadius);
            Vector3[] xySecond = DrawCircleQuarter(center, worldRadius, Quadrant.Second, Enums.Plane.XY, pointsPerRadius);
            Vector3[] xyThird = DrawCircleQuarter(center, worldRadius, Quadrant.Third, Enums.Plane.XY, pointsPerRadius);
            Vector3[] xyFourth = DrawCircleQuarter(center, worldRadius, Quadrant.Fourth, Enums.Plane.XY, pointsPerRadius);

            return ArrayExtensions.Merge(xz, xyFirst, yz, xySecond, xyThird, xyFourth);
        }

        #endregion

        #region capsules
        public static Vector3[] DrawFibonacciCapsule(Vector3 center, float worldRadius, float worldHeight, float pointsPerUnit = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(worldRadius), "Radius should be positive");

            if (worldHeight < 0)
                throw new ArgumentOutOfRangeException(nameof(worldHeight), "Height should be positive");

            if (pointsPerUnit <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerUnit), "Points per unit should be positive");

            float cylinderHeight = Mathf.Max(0f, worldHeight - 2f * worldRadius);
            int spherePoints = Mathf.RoundToInt(pointsPerUnit * worldRadius);
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
                    points.Add(sphereCenter + dir * worldRadius);
                }
                else // bottom one
                {
                    Vector3 sphereCenter = center - Vector3.up * (cylinderHeight / 2f);
                    points.Add(sphereCenter + dir * worldRadius);
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

                points.Add(center + new Vector3(x * worldRadius, y, z * worldRadius));
            }

            return points.ToArray();
        }

        public static Vector3[] DrawLatitudeLongitudeCapsule(Vector3 center, float worldRadius, float worldHeight, float pointsPerUnit = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(worldRadius), "Radius should be positive");

            if (worldHeight < 0)
                throw new ArgumentOutOfRangeException(nameof(worldHeight), "Height should be positive");

            if (pointsPerUnit <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerUnit), "Points per unit should be positive");

            float cylinderHeight = Mathf.Max(0f, worldHeight - 2f * worldRadius);

            int totalPoints = Mathf.RoundToInt(pointsPerUnit * (worldRadius + cylinderHeight));
            int latSteps = Mathf.Max(4, Mathf.RoundToInt(Mathf.Sqrt(totalPoints / 2f)));
            int lonSteps = latSteps * 2;

            List<Vector3> points = new List<Vector3>();

            float lonStep = 2f * Mathf.PI / lonSteps;

            
            // Top hemisphere
            int hemiLatSteps = latSteps / 2;
            float latStep = MathConstants.HALF_PI / hemiLatSteps;

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
                            worldRadius * sinA * Mathf.Cos(b),
                            worldRadius * cosA,
                            worldRadius * sinA * Mathf.Sin(b)
                        )
                    );
                }
            }

           

            // Cyllinder
            int cylinderSteps = Mathf.Max(1, Mathf.RoundToInt(pointsPerUnit * cylinderHeight / worldRadius));
            float yStep = cylinderHeight / cylinderSteps;

            for (int i = 1; i < cylinderSteps; i++)
            {
                float y = -cylinderHeight / 2f + i * yStep;

                for (int j = 0; j < lonSteps; j++)
                {
                    float b = j * lonStep;

                    points.Add(
                        center + new Vector3(
                            worldRadius * Mathf.Cos(b),
                            y,
                            worldRadius * Mathf.Sin(b)
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
                            worldRadius * sinA * Mathf.Cos(b),
                            -worldRadius * cosA,
                            worldRadius * sinA * Mathf.Sin(b)
                        )
                    );
                }
            }

            return points.ToArray();
        }
        #endregion

        #region circles
        public static Vector3[] DrawCircleQuarter(Vector3 center, float worldRadius, Quadrant quadrant, Enums.Plane plane, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            int pointsCount = Mathf.RoundToInt(pointsPerRadius * worldRadius);
            Vector3[] points = new Vector3[pointsCount];
            float step = MathConstants.HALF_PI / pointsCount;

            quadrant.GetMinMax(out float min, out float max);
            int index = 0;

            for (float f = min; f <= max; f += step)
            {
                if (index >= pointsCount)
                    break;
                
                Vector3 vector = plane switch
                {
                    Enums.Plane.XY => new Vector3(center.x + worldRadius * Mathf.Cos(f), center.y + worldRadius * Mathf.Sin(f), center.z),
                    Enums.Plane.XZ => new Vector3(center.x + worldRadius * Mathf.Cos(f), center.y, center.z + worldRadius * Mathf.Sin(f)),
                    Enums.Plane.YZ => new Vector3(center.x, center.y + worldRadius * Mathf.Cos(f), center.z + worldRadius * Mathf.Sin(f)),
                    _ => throw new ArgumentException($"Unknown plane! {plane}"),
                };
                points[index++] = vector;
            }

            return points;
        }

        public static Vector3[] DrawCircle(Vector3 center, float worldRadius, Enums.Plane plane, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            int pointsCount = Mathf.RoundToInt(pointsPerRadius * worldRadius);
            Vector3[] points = new Vector3[pointsCount];
            float step = MathConstants.TWO_PI / pointsCount;

            for (int i = 0; i < pointsCount; i++)
            {
                float f = i * step;
                Vector3 vector = plane switch
                {
                    Enums.Plane.XY => new Vector3(center.x + worldRadius * Mathf.Cos(f), center.y + worldRadius * Mathf.Sin(f), center.z),
                    Enums.Plane.XZ => new Vector3(center.x + worldRadius * Mathf.Cos(f), center.y, center.z + worldRadius * Mathf.Sin(f)),
                    Enums.Plane.YZ => new Vector3(center.x, center.y + worldRadius * Mathf.Cos(f), center.z + worldRadius * Mathf.Sin(f)),
                    _ => throw new ArgumentException($"Unknown plane! {plane}"),
                };

                points[i] = vector;
            }

            return points;
        }
        #endregion
    }
}
