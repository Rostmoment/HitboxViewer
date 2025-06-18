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

        public static void SetPositions(LineRenderer line, List<Vector3> vectors) => SetPositions(line, vectors.ToArray());
        public static void SetPositions(LineRenderer line, params Vector3[] positions)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }

        protected void CreateParent()
        {
            if (parentObject == null)
            {
                parentObject = new GameObject("Hitbox Viewer Parent Object");
                parentObject.transform.SetParent(gameObject.transform);
            }
        }
        protected virtual LineRenderer CreateLineRendered<T>(T collider, Dictionary<T, LineRenderer> renderers)
        {
            CreateParent();
            if (!renderers.TryGetValue(collider, out LineRenderer line))
            {
                Color color = HitboxViewConfig.GetHitboxColor(collider);
                if (color.a == 0)
                    return null;
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

        public void DrawCapsule(Transform tranformOfHitbox, float localRadius, float localHeight, Vector3 localCenter)
        {
            Vector3 worldScale = tranformOfHitbox.lossyScale;
            Vector3 center = tranformOfHitbox.TransformPoint(localCenter);
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = localHeight * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            int pointsCount = Mathf.RoundToInt(localRadius * HitboxViewConfig.PointsPerRadius);
            int pointsPerSegment = pointsCount / 8;
            float centerOffset = Mathf.Abs((height - 2 * radius) / 2);

            // upper half-circle
            float newY = center.y + centerOffset;
            float step = Mathf.PI * 2 / (pointsPerSegment * 2);
            for (float i = 0; i <= Mathf.PI * 2; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Cos(i), newY, center.z + radius * Mathf.Sin(i)));

            // Ellipse by Z
            step = Mathf.PI / (pointsPerSegment * 2);
            for (float i = 0; i <= Mathf.PI; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Cos(i), newY + radius * Mathf.Sin(i), center.z));

            newY = center.y - centerOffset;
            for (float i = Mathf.PI; i <= Mathf.PI * 2; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Cos(i), newY + radius * Mathf.Sin(i), center.z));

            // lower half-circle
            step = Mathf.PI * 2 / (pointsPerSegment * 2);
            for (float i = 0; i <= Mathf.PI * 2; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Cos(i), newY, center.z + radius * Mathf.Sin(i)));
            positions.Add(new Vector3(center.x + radius, newY, center.z));
            newY = center.y + centerOffset;
            // Connect with upper half-circle
            positions.Add(new Vector3(center.x + radius, newY, center.z));

            // Go to upper point
            step = Mathf.PI * 2 / (pointsPerSegment * 2);
            for (float i = 0; i <= Mathf.PI / 2; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Cos(i), newY, center.z + radius * Mathf.Sin(i)));

            // Ellipse by X
            for (float i = 0; i <= Mathf.PI; i += step)
                positions.Add(new Vector3(center.x, newY + radius * Mathf.Sin(i), center.z + radius * Mathf.Cos(i)));
            positions.Add(new Vector3(center.x, newY, center.z - radius));

            newY = center.y - centerOffset;
            for (float i = Mathf.PI; i <= Mathf.PI * 2; i += step)
                positions.Add(new Vector3(center.x, newY + radius * Mathf.Sin(i), center.z + radius * Mathf.Cos(i)));

            newY = center.y + centerOffset;
            positions.Add(new Vector3(center.x, newY, center.z + radius));

            positions.Rotate(center, tranformOfHitbox.transform.rotation);
            SetPositions(lineRenderer, positions);
        }
    }
}
