using HitboxViewer.Configs;
using HitboxViewer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Displayers.Helpers
{
    public static class CircleDisplayerHelper
    {
        public static Vector3[] DrawCircleQuarter(Vector3 center, float worldRadius, Quadrant quadrant, Enums.Plane plane, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            int pointsCount = Mathf.RoundToInt(pointsPerRadius * worldRadius);

            Vector3[] points = new Vector3[pointsCount];

            quadrant.GetMinMax(out float min, out float max);

            for (int i = 0; i < pointsCount; i++)
            {
                float t = (float)i / (pointsCount - 1);
                float f = Mathf.Lerp(min, max, t);

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

        public static void DrawAllCircleQuarters(Vector3 center, float worldRadius, Enums.Plane plane, out Vector3[] first, out Vector3[] second, out Vector3[] third, out Vector3[] fouth, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            first = DrawCircleQuarter(center, worldRadius, Quadrant.First, plane, pointsPerRadius);
            second = DrawCircleQuarter(center, worldRadius, Quadrant.Second, plane, pointsPerRadius);
            third = DrawCircleQuarter(center, worldRadius, Quadrant.Third, plane, pointsPerRadius);
            fouth = DrawCircleQuarter(center, worldRadius, Quadrant.Fourth, plane, pointsPerRadius);
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
        public static Vector3[] DrawCircle(Vector3 center, float worldRadius, Enums.Plane plane, Quadrant quadrant, float pointsPerRadius = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT)
        {
            int pointsCount = Mathf.RoundToInt(pointsPerRadius * worldRadius);
            Vector3[] points = new Vector3[pointsCount];
            float step = MathConstants.TWO_PI / pointsCount;
            quadrant.GetMinMax(out float offset, out _);

            for (int i = 0; i < pointsCount; i++)
            {
                float f = i * step + offset;
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
    }
}
