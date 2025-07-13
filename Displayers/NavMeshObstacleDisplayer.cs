using HarmonyLib;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer.Displayers
{
    class NavMeshObstacleDisplayer : BaseDisplayer
    {
        private static List<NavMeshObstacleDisplayer> all = new List<NavMeshObstacleDisplayer>();
        private static Dictionary<NavMeshObstacle, LineRenderer> renderers = new Dictionary<NavMeshObstacle, LineRenderer>();

        private void Start()
        {
            all.Add(this);
        }
        private void OnDestroy()
        {
            all.Remove(this);
        }
        public static void HideAll()
        {
            renderers = ClearFromNull(renderers);
            LineRenderer[] lines = renderers.Values.ToArray();
            for (int i = 0; i < lines.Length; i++)
                lines[i].gameObject.SetActive(false);
        }
        protected override LineRenderer CreateLineRendered<T>(T collider, Dictionary<T, LineRenderer> renderers)
        {
            if (collider.GetType() != typeof(NavMeshObstacle))
            {
                BasePlugin.Logger.LogWarning($"{collider.GetType()} is not NavMeshObstacle");
                return null;
            }
            return base.CreateLineRendered(collider, renderers);
        }
        public static void Show()
        {
            bool box = BasePlugin.NavMeshObstacleVisualize == NavMeshObstacleVisualizationMode.Box;
            bool capsule = BasePlugin.NavMeshObstacleVisualize == NavMeshObstacleVisualizationMode.Capsule;
            if (BasePlugin.NavMeshObstacleVisualize == NavMeshObstacleVisualizationMode.All)
            {
                box = true;
                capsule = true;
            }
            renderers = ClearFromNull(renderers); 
            foreach (var data in renderers)
            {
                try
                {
                    if (box && data.Key.shape == NavMeshObstacleShape.Box)
                        data.Value.gameObject.SetActive(true);
                    if (capsule && data.Key.shape == NavMeshObstacleShape.Capsule)
                        data.Value.gameObject.SetActive(true);
                }
                catch (NullReferenceException) { }
            }
        }
        public override void Visualize()
        {
            base.Visualize();
            gameObject.GetComponents<NavMeshObstacle>().Do(x => InitializeGlobal(x));
        }
        public override void InitializeGlobal<T>(T collider)
        {
            NavMeshObstacle obstacle = collider as NavMeshObstacle;
            if (obstacle == null)
                return;
            lineRenderer = CreateLineRendered<NavMeshObstacle>(obstacle, renderers);
            if (lineRenderer == null || !lineRenderer.enabled)
                return;
            positions.Clear();
            ShowBox(obstacle);
            ShowCapsule(obstacle);

        }
        private void ShowBox(NavMeshObstacle obstacle)
        {
            if (obstacle.shape != NavMeshObstacleShape.Box)
                return;
            Vector3[] vertices = obstacle.GetVertices();
            SetPositions(lineRenderer, vertices[0], vertices[1], vertices[5], vertices[4], vertices[0], vertices[2], vertices[3], vertices[7],
                vertices[5], vertices[4], vertices[6], vertices[7], vertices[6], vertices[2], vertices[3], vertices[1]);
        }
        private void ShowCapsule(NavMeshObstacle obstacle)
        {
            if (obstacle.shape != NavMeshObstacleShape.Capsule)
                return;

            DrawCapsule(obstacle.transform, obstacle.radius, obstacle.height, obstacle.center);
        }


        public static void UpdatePre()
        {
            NavMeshObstacleDisplayer.all.RemoveAll(x => x == null);
            if (Input.GetKeyDown(HitboxViewConfig.ChangeNavMeshObstacleVisualizeMode))
            {
                BasePlugin.NavMeshObstacleVisualize = BasePlugin.NavMeshObstacleVisualize.Next();
                NavMeshObstacleDisplayer.HideAll();
                NavMeshObstacleDisplayer.Show();
            }
            if (BasePlugin.NavMeshObstacleVisualize == NavMeshObstacleVisualizationMode.Hide)
                NavMeshObstacleDisplayer.HideAll();
        }
    }
}
