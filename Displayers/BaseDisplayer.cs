using BepInEx.Configuration;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace HitboxViewer.Displayers
{
    class BaseDisplayer : MonoBehaviour
    {
        protected static List<Vector3> positions = new List<Vector3>();
        protected static LineRenderer lineRenderer;
        protected GameObject parentObject;

        private void Awake()
        {
            VirtualAwake();
        }
        protected virtual void VirtualAwake()
        {
            CreateParent();
        }
        private void OnDestroy()
        {
            VirtualOnDestroy();
        }
        protected virtual void VirtualOnDestroy()
        {

        }

        public static Dictionary<T, K> ClearFromNull<T, K>(Dictionary<T, K> dictionary) => dictionary.Where(x => !x.Value.IsNullOrDestroyed() && !x.Key.IsNullOrDestroyed()).ToDictionary(x => x.Key, x => x.Value);
        public static void SetPositions(LineRenderer line, List<Vector3> vectors) => SetPositions(line, vectors.ToArray());
        public static void SetPositions(LineRenderer line, params Vector3[] positions)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }

        protected virtual void CreateParent()
        {
            if (parentObject == null)
            {
                parentObject = new GameObject("Hitbox Viewer Parent Object");
                parentObject.transform.SetParent(gameObject.transform);
            }
        }
        protected virtual LineRenderer CreateLineRendered<T>(T collider, Dictionary<T, LineRenderer> renderers)
        {
            if (!renderers.TryGetValue(collider, out LineRenderer line))
            {
                CreateParent();
                Color color = HitboxViewConfig.GetHitboxColor(collider);
                GameObject game = new GameObject("LineRenderer Hitbox Viewer");
                game.transform.SetParent(parentObject.transform);
                Gradient gradient = new Gradient();
                gradient.SetKeys([new GradientColorKey(color, 0), new GradientColorKey(color, 1)],
                    [new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)]);
                line = game.AddComponent<LineRenderer>();
                line.colorGradient = gradient;
                line.endColor = color;
                line.startColor = color;
                line.startWidth = HitboxViewConfig.HitboxLineWidth;
                line.endWidth = HitboxViewConfig.HitboxLineWidth;
                line.material = new Material(Shader.Find(HitboxViewConfig.ShaderHitboxName))
                {
                    color = color
                };
                renderers.Add(collider, line);
            }
            return line;
        }
        public virtual void Visualize()
        {

        }
        public virtual void InitializeGlobal<T>(T collider)
        {

        }

        public void DrawSphere(Vector3 center, float localRadius, Transform hitboxTransform)
        {
            Vector3 worldScale = hitboxTransform.lossyScale;
            Vector3 worldPosition = hitboxTransform.position;

            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            int pointsCount = Mathf.RoundToInt(radius * HitboxViewConfig.PointsPerRadius);

            float step = HitboxViewConfig.SphereVisualizationMode switch
            {
                SphereVisualizationMode.Full => 4 * Mathf.PI * radius * radius / pointsCount,
                SphereVisualizationMode.ThreeAxis => 6 * Mathf.PI * radius / pointsCount,
                SphereVisualizationMode.TwoAxis => 4 * Mathf.PI * radius / pointsCount,
                _ => throw new ArgumentException($"Unknown SphereVisualizationMode!\n{HitboxViewConfig.SphereVisualizationMode}"),
            };

            #region Full
            if (HitboxViewConfig.SphereVisualizationMode == SphereVisualizationMode.Full)
            {
                for (float a = 0; a <= Mathf.PI; a += step)
                {
                    float sin = Mathf.Sin(a);
                    float cos = Mathf.Cos(a);
                    for (float b = 0; b <= 2 * Mathf.PI; b += step)
                    {
                        positions.Add(center + new Vector3(radius * sin * Mathf.Cos(b),
                            radius * sin * Mathf.Sin(b),
                            radius * cos));
                    }
                }
            }
            #endregion

            #region 3 and 2 axis
            else
            {
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XY, Quadrant.First, step));
                if (HitboxViewConfig.SphereVisualizationMode == SphereVisualizationMode.ThreeAxis)
                {
                    positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.YZ, Quadrant.First, step));
                    positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.YZ, Quadrant.Second, step));
                    positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.YZ, Quadrant.Third, step));
                    positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.YZ, Quadrant.Fourth, step));
                }
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XY, Quadrant.Second, step));

                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XZ, Quadrant.First, step));
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XZ, Quadrant.Second, step));
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XZ, Quadrant.Third, step));
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XZ, Quadrant.Fourth, step));

                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XY, Quadrant.Third, step));
                positions.AddRange(DrawCircleQuarter(hitboxTransform, localRadius, center, Plane.XY, Quadrant.Fourth, step));
            }
            #endregion
            SetPositions(lineRenderer, positions);
        }
        public Vector3[] DrawCircleQuarter(Transform transformOfHitbox, float localRadius, Vector3 center, Plane plane, Quadrant quadrant, float step = float.NaN, bool reverse = false)
        {
            Vector3 worldScale = transformOfHitbox.lossyScale;
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            if (float.IsNaN(step))
                step = 2 * Mathf.PI * radius / HitboxViewConfig.PointsPerRadius;

            Vector2 minmax = quadrant switch
            {
                Quadrant.First => new Vector2(0, Mathf.PI / 2),
                Quadrant.Second => new Vector2(Mathf.PI / 2, Mathf.PI),
                Quadrant.Third => new Vector2(Mathf.PI, 3 * Mathf.PI / 2),
                Quadrant.Fourth => new Vector2(3 * Mathf.PI / 2, 2 * Mathf.PI),
                _ => throw new ArgumentException($"Unknown quadrant! --- '{quadrant}'"),
            };

            List<Vector3> result = [];
            for (float i = minmax.x; i <= minmax.y + step / 2; i += step)
            {
                switch (plane)
                {
                    case Plane.XY:
                        result.Add(new Vector3(center.x + radius * Mathf.Cos(i), center.y + radius * Mathf.Sin(i), center.z));
                        break;
                    case Plane.XZ:
                        result.Add(new Vector3(center.x + radius * Mathf.Cos(i), center.y, center.z + radius * Mathf.Sin(i)));
                        break;
                    case Plane.YZ:
                        result.Add(new Vector3(center.x, center.y + radius * Mathf.Cos(i), center.z + radius * Mathf.Sin(i)));
                        break;
                    default:
                        throw new ArgumentException($"Unknown plane! --- '{plane}'");
                }
            }
            switch (plane)
            {
                case Plane.XY:
                    result.Add(new Vector3(center.x + radius * Mathf.Cos(minmax.y), center.y + radius * Mathf.Sin(minmax.y), center.z));
                    break;
                case Plane.XZ:
                    result.Add(new Vector3(center.x + radius * Mathf.Cos(minmax.y), center.y , center.z + radius * Mathf.Sin(minmax.y)));
                    break;
                case Plane.YZ:
                    result.Add(new Vector3(center.x, center.y + radius * Mathf.Cos(minmax.y), center.z + radius * Mathf.Sin(minmax.y)));
                    break;
                default:
                    throw new ArgumentException($"Unknown plane! --- '{plane}'");
            }
            if (reverse)
                result.Reverse();
            return result.ToArray();    
        }
        protected void DrawRectTransform(RectTransform rectTransform)
        {
            if (rectTransform == null) return;

            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            positions.Clear();
            positions.Add(worldCorners[0]);
            positions.Add(worldCorners[1]);
            positions.Add(worldCorners[2]);
            positions.Add(worldCorners[3]);
            positions.Add(worldCorners[0]);

            SetPositions(lineRenderer, positions);
        }
        public void DrawCapsuleSurface(Transform transformOfHitbox, float localRadius, float localHeight, Vector3 localCenter)
        {
            Vector3 worldScale = transformOfHitbox.lossyScale;
            Vector3 center = transformOfHitbox.TransformPoint(localCenter);
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = localHeight * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            // The capsule axis is assumed to be aligned with the local Y axis
            float cylinderLength = Mathf.Max(0, height - 2 * radius);

            int latSegments = Mathf.RoundToInt(radius * HitboxViewConfig.PointsPerRadius); // latitude (from pole to equator)
            int lonSegments = latSegments * 2; // longitude (around the body)
            int heightSegments = Mathf.RoundToInt(cylinderLength * HitboxViewConfig.PointsPerRadius / radius);

            // Top hemisphere
            for (int lat = 0; lat <= latSegments; lat++)
            {
                float theta = Mathf.PI / 2 * (lat / (float)latSegments); // 0 (equator) to pi/2 (pole)
                float y = Mathf.Sin(theta) * radius;
                float r = Mathf.Cos(theta) * radius;
                for (int lon = 0; lon < lonSegments; lon++)
                {
                    float phi = 2 * Mathf.PI * (lon / (float)lonSegments);
                    float x = Mathf.Cos(phi) * r;
                    float z = Mathf.Sin(phi) * r;
                    Vector3 pt = center + Vector3.up * (cylinderLength / 2 + y) + new Vector3(x, 0, z);
                    positions.Add(transformOfHitbox.TransformDirection(pt - center) + center);
                }
            }

            // Cylinder body
            for (int h = 1; h < heightSegments; h++)
            {
                float y = cylinderLength * (h / (float)heightSegments) - cylinderLength / 2;
                for (int lon = 0; lon < lonSegments; lon++)
                {
                    float phi = 2 * Mathf.PI * (lon / (float)lonSegments);
                    float x = Mathf.Cos(phi) * radius;
                    float z = Mathf.Sin(phi) * radius;
                    Vector3 pt = center + Vector3.up * y + new Vector3(x, 0, z);
                    positions.Add(transformOfHitbox.TransformDirection(pt - center) + center);
                }
            }

            // Bottom hemisphere
            for (int lat = 0; lat <= latSegments; lat++)
            {
                float theta = Mathf.PI / 2 * (lat / (float)latSegments); // 0 (equator) to pi/2 (pole)
                float y = -Mathf.Sin(theta) * radius;
                float r = Mathf.Cos(theta) * radius;
                for (int lon = 0; lon < lonSegments; lon++)
                {
                    float phi = 2 * Mathf.PI * (lon / (float)lonSegments);
                    float x = Mathf.Cos(phi) * r;
                    float z = Mathf.Sin(phi) * r;
                    Vector3 pt = center + Vector3.down * (cylinderLength / 2) + Vector3.up * y + new Vector3(x, 0, z);
                    positions.Add(transformOfHitbox.TransformDirection(pt - center) + center);
                }
            }

            SetPositions(lineRenderer, positions);
        }
        public void DrawCapsule(Transform transformOfHitbox, float localRadius, float localHeight, Vector3 localCenter)
        {
            Vector3 worldScale = transformOfHitbox.lossyScale;
            Vector3 center = transformOfHitbox.TransformPoint(localCenter);
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = localHeight * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float centerOffset = Mathf.Abs((height - 2 * radius) / 2);

            // Top circle
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.First));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Second));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Third));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Fourth));

            // XY Ellipse start
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XY, Quadrant.First));

            // YX Ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.YZ, Quadrant.First));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.YZ, Quadrant.Second));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.YZ, Quadrant.Third));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.YZ, Quadrant.Fourth));

            // Other part of XY ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XY, Quadrant.Second));

            // Bottom circle
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Third));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Fourth));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.First));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Second));

            // Last part of XY ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XY, Quadrant.Third));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XY, Quadrant.Fourth));

            // First point
            positions.Add(positions[0]);

            positions.Rotate(center, transformOfHitbox.rotation);
            SetPositions(lineRenderer, positions);
        }

    }
}
