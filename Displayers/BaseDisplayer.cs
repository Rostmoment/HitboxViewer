using BepInEx.Configuration;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public static Dictionary<T, K> ClearFromNull<T, K>(Dictionary<T, K> dictionary) => dictionary.Where(x => x.Value != null && x.Key != null).ToDictionary(x => x.Key, x => x.Value);
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

        public Vector3[] DrawCircleQuarter(Transform transformOfHitbox, float localRadius, Vector3 center, Plane plane, Quadrant quadrant, float step = float.NaN, bool reverse = false)
        {
            Vector3 worldScale = transformOfHitbox.lossyScale;
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            if (float.IsNaN(step))
                step = 2 * Mathf.PI / HitboxViewConfig.PointsPerRadius;

            Vector2 minmax = new Vector2();
            switch (quadrant)
            {
                case Quadrant.First:
                    minmax = new Vector2(0, Mathf.PI / 2);
                    break;
                case Quadrant.Second:
                    minmax = new Vector2(Mathf.PI / 2, Mathf.PI);
                    break;
                case Quadrant.Third:
                    minmax = new Vector2(Mathf.PI, 3 * Mathf.PI / 2);
                    break;
                case Quadrant.Fourth:
                    minmax = new Vector2(3 * Mathf.PI / 2, 2 * Mathf.PI);
                    break;
                default:
                    return [BasePlugin.VectorNaN];
            }

            List<Vector3> result = new List<Vector3>();
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
                        return [BasePlugin.VectorNaN];
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
                    break;
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

        public void DrawCapsule(Transform transformOfHitbox, float localRadius, float localHeight, Vector3 localCenter)
        {
            Vector3 worldScale = transformOfHitbox.lossyScale;
            Vector3 center = transformOfHitbox.TransformPoint(localCenter);
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = localHeight * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float centerOffset = Mathf.Abs((height - 2 * radius) / 2);


            float step = 2 * Mathf.PI / HitboxViewConfig.PointsPerRadius;

            // Top circle
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.First, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Second, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Third, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XZ, Quadrant.Fourth, step));

            // XY Ellipse start
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XY, Quadrant.First, step));

            // YX Ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.YZ, Quadrant.First, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.YZ, Quadrant.Second, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.YZ, Quadrant.Third, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.YZ, Quadrant.Fourth, step));

            // Other part of XY ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center + Vector3.up * centerOffset, Plane.XY, Quadrant.Second, step));

            // Bottom circle
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Third, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Fourth, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.First, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XZ, Quadrant.Second, step));

            // Last part of XY ellipse
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XY, Quadrant.Third, step));
            positions.AddRange(DrawCircleQuarter(transformOfHitbox, localRadius, center - Vector3.up * centerOffset, Plane.XY, Quadrant.Fourth, step));

            // First point
            positions.Add(positions[0]);

            positions.Rotate(center, transformOfHitbox.rotation);
            SetPositions(lineRenderer, positions);
        }

    }
}
