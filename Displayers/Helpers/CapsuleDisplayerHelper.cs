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

namespace HitboxViewer.Displayers.Helpers
{
    public static class CapsuleDisplayerHelper
    {

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

        public static Vector3[] DrawTwoAxisCapsule(Vector3 center, float worldRadius, float worldHeight, float pointsPerUnit = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            if (worldRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(worldRadius), "Radius should be positive");

            if (worldHeight < 0)
                throw new ArgumentOutOfRangeException(nameof(worldHeight), "Height should be positive");

            if (pointsPerUnit <= 0)
                throw new ArgumentOutOfRangeException(nameof(pointsPerUnit), "Points per unit should be positive");

            float centerOffset = Mathf.Abs((worldHeight - 2 * worldRadius) / 2);
            Vector3 topSphereCetner = center + Vector3.up * centerOffset;
            Vector3 bottomSphereCetner = center - Vector3.up * centerOffset;

            Vector3[] topCircle = CircleDisplayerHelper.DrawCircle(topSphereCetner, worldRadius, Enums.Plane.XZ, pointsPerUnit);
            Vector3[] bottomCircle = CircleDisplayerHelper.DrawCircle(bottomSphereCetner, worldRadius, Enums.Plane.XZ, Quadrant.Third, pointsPerUnit);

            Vector3[] xyFirstTop = CircleDisplayerHelper.DrawCircleQuarter(topSphereCetner, worldRadius, Quadrant.First, Enums.Plane.XY, pointsPerUnit);
            Vector3[] xySecondTop = CircleDisplayerHelper.DrawCircleQuarter(topSphereCetner, worldRadius, Quadrant.Second, Enums.Plane.XY, pointsPerUnit);
            Vector3[] xyFirstBottom = CircleDisplayerHelper.DrawCircleQuarter(bottomSphereCetner, worldRadius, Quadrant.Third, Enums.Plane.XY, pointsPerUnit); // actually third quadrant because previous is second
            Vector3[] xySecondBottom = CircleDisplayerHelper.DrawCircleQuarter(bottomSphereCetner, worldRadius, Quadrant.Fourth, Enums.Plane.XY, pointsPerUnit);

            return ArrayExtensions.Merge(topCircle, xyFirstTop, xySecondTop, bottomCircle, xyFirstBottom, xySecondBottom, [topCircle[0]]);
        }

    }
}
