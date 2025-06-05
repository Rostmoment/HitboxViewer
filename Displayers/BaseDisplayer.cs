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
    }
}
