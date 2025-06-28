using HarmonyLib;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

namespace HitboxViewer.Displayers
{
    class ColliderDisplayer : BaseDisplayer
    {
        private static Dictionary<Type, UnityAction<ColliderDisplayer, object>> initialize = new Dictionary<Type, UnityAction<ColliderDisplayer, object>>()
        {
            {typeof(BoxCollider2D),  (disaplyer, collider) => disaplyer.Initialize((BoxCollider2D)collider)},
            {typeof(CircleCollider2D),  (disaplyer, collider) => disaplyer.Initialize((CircleCollider2D)collider)},
            {typeof(CapsuleCollider2D),  (disaplyer, collider) => disaplyer.Initialize((CapsuleCollider2D)collider)},
            {typeof(PolygonCollider2D),  (disaplyer, collider) => disaplyer.Initialize((PolygonCollider2D)collider)},
            {typeof(EdgeCollider2D),  (disaplyer, collider) => disaplyer.Initialize((EdgeCollider2D)collider)},
            {typeof(CompositeCollider2D),  (disaplyer, collider) => disaplyer.Initialize((CompositeCollider2D)collider)},
            {typeof(BoxCollider),  (disaplyer, collider) => disaplyer.Initialize((BoxCollider)collider)},
            {typeof(SphereCollider),  (disaplyer, collider) => disaplyer.Initialize((SphereCollider)collider)},
            {typeof(CapsuleCollider),  (disaplyer, collider) => disaplyer.Initialize((CapsuleCollider)collider)},
            {typeof(CharacterController),  (disaplyer, collider) => disaplyer.Initialize((CharacterController)collider)},
            {typeof(MeshCollider),  (disaplyer, collider) => disaplyer.Initialize((MeshCollider)collider)},
            {typeof(WheelCollider),  (disaplyer, collider) => disaplyer.Initialize((WheelCollider)collider)},
            {typeof(TerrainCollider),  (disaplyer, collider) => disaplyer.Initialize((TerrainCollider)collider)}
        };
        private static Dictionary<object, LineRenderer> renderers = new Dictionary<object, LineRenderer>();
        private static List<ColliderDisplayer> all = new List<ColliderDisplayer>();

        protected override void VirtualAwake()
        {
            base.VirtualAwake();
            all.Add(this);
        }
        protected override void VirtualOnDestroy()
        {
            all.Remove(this);
        }
        public static void HideAll()
        {
            renderers = renderers.Where(x => x.Value != null && x.Key != null).ToDictionary(x => x.Key, x => x.Value);
            LineRenderer[] lines = renderers.Values.ToArray();
            for (int i = 0; i < lines.Length; i++)
                lines[i].gameObject.SetActive(false);
        }
        public static void Show()
        {
            bool showCollider = BasePlugin.ColliderVisualize == CollidersVisualizationMode.NotTrigger;
            bool showTrigger = BasePlugin.ColliderVisualize == CollidersVisualizationMode.Trigger;
            if (BasePlugin.ColliderVisualize == CollidersVisualizationMode.All)
            {
                showCollider = true;
                showTrigger = true;
            }
            renderers = renderers.Where(x => x.Value != null && x.Key != null).ToDictionary(x => x.Key, x => x.Value);
            for (int i = 0; i < renderers.Count; i++)
            {
                var data = renderers.ElementAt(i);
                try
                {
                    if (data.Key is Collider collider)
                    {
                        if (showTrigger && collider.isTrigger)
                            data.Value.gameObject.SetActive(true);
                        if (showCollider && !collider.isTrigger)
                            data.Value.gameObject.SetActive(true);
                    }
                    if (data.Key is Collider2D collider2D)
                    {
                        if (showTrigger && collider2D.isTrigger)
                            data.Value.gameObject.SetActive(true);
                        if (showCollider && !collider2D.isTrigger)
                            data.Value.gameObject.SetActive(true);
                    }
                }
                catch (NullReferenceException) { }
            }
        }
        protected override LineRenderer CreateLineRendered<T>(T collider, Dictionary<T, LineRenderer> renderers)
        {
            if (collider is not Collider2D && collider is not Collider)
            {
                BasePlugin.Logger.LogWarning($"{collider.GetType()} is not Collider or Collider2D!");
                return null;
            }
            return base.CreateLineRendered(collider, renderers);

        }
        public override void Visualize()
        {
            List<Collider> colliders = gameObject.GetComponents<Collider>().ToList();
            List<Collider2D> colliders2D = gameObject.GetComponents<Collider2D>().ToList();
            switch (BasePlugin.ColliderVisualize)
            {
                case CollidersVisualizationMode.Hide:
                    return;
                case CollidersVisualizationMode.Trigger:
                    colliders.RemoveAll(x => !x.isTrigger);
                    colliders2D.RemoveAll(x => !x.isTrigger);
                    break;
                case CollidersVisualizationMode.NotTrigger:
                    colliders.RemoveAll(x => x.isTrigger);
                    colliders2D.RemoveAll(x => x.isTrigger);
                    break;
                case CollidersVisualizationMode.All:
                    break;
                default:
                    break;
            }
            colliders.Do(x => InitializeGlobal(x));
            colliders2D.Do(x => InitializeGlobal(x));
        }

        public override void InitializeGlobal<T>(T collider)
        {
            lineRenderer = CreateLineRendered(collider, renderers);
            if (lineRenderer == null || !lineRenderer.enabled || !initialize.TryGetValue(collider.GetType(), out var action))
                return;
            positions.Clear();
            action.Invoke(this, collider);
        }


        public static void UpdatePre()
        {
            ColliderDisplayer.all.RemoveAll(x => x == null);
            if (Input.GetKeyDown(HitboxViewConfig.ChangeColliderVisualizeMode))
            {
                BasePlugin.ColliderVisualize = (CollidersVisualizationMode)(((int)BasePlugin.ColliderVisualize + 1) % 4);
                ColliderDisplayer.HideAll();
                ColliderDisplayer.Show();
            }
            if (BasePlugin.ColliderVisualize == CollidersVisualizationMode.Hide)
            {
                ColliderDisplayer.HideAll();
            }
        }



        public void Initialize(BoxCollider collider)
        {
            Vector3[] vertices = collider.GetVertices();    
            SetPositions(lineRenderer, vertices[0], vertices[1], vertices[5], vertices[4], vertices[0], vertices[2], vertices[3], vertices[7],
                vertices[5], vertices[4], vertices[6], vertices[7], vertices[6], vertices[2], vertices[3], vertices[1]);
        }
        public void Initialize(SphereCollider collider)
        {
            float localRadius = collider.radius;
            Vector3 worldScale = collider.transform.lossyScale;
            Vector3 worldPosition = collider.transform.position;
            float worldRadius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            Vector3 worldCenter = collider.transform.TransformPoint(collider.center);
            int pointsCount = Mathf.RoundToInt(worldRadius * HitboxViewConfig.PointsPerRadius);
            float step = Mathf.PI * Mathf.Sqrt(2 * pointsCount) / pointsCount;
            for (float a = 0; a <= Mathf.PI; a += step)
            {
                float sin = Mathf.Sin(a);
                float cos = Mathf.Cos(a);
                for (float b = 0; b <= 2 * Mathf.PI; b += step)
                {
                    positions.Add(worldCenter + new Vector3(worldRadius * sin * Mathf.Cos(b),
                        worldRadius * sin * Mathf.Sin(b),
                        worldRadius * cos));
                }
            }
            SetPositions(lineRenderer, positions);
        }

        public void Initialize(CharacterController characterController) => DrawCapsule(characterController.transform, characterController.height, characterController.height, characterController.center);
        public void Initialize(CapsuleCollider capsuleCollider) => DrawCapsule(capsuleCollider.transform, capsuleCollider.height, capsuleCollider.height, capsuleCollider.center);

        public void Initialize(MeshCollider collider)
        {
            if (collider.sharedMesh == null)
                return;

            Mesh mesh = collider.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;


            HashSet<(int, int)> drawnEdges = new HashSet<(int, int)>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int i0 = triangles[i];
                int i1 = triangles[i + 1];
                int i2 = triangles[i + 2];

                DrawEdge(i0, i1);
                DrawEdge(i1, i2);
                DrawEdge(i2, i0);
            }
            positions.Add(positions[0]);
            SetPositions(lineRenderer, positions);

            void DrawEdge(int a, int b)
            {
                (int, int) edge = (Mathf.Min(a, b), Mathf.Max(a, b));
                if (!drawnEdges.Add(edge))
                    return;

                Vector3 worldA = collider.transform.TransformPoint(vertices[a]);
                Vector3 worldB = collider.transform.TransformPoint(vertices[b]);
                positions.Add(worldA);
                positions.Add(worldB);
            }
        }

        public void Initialize(WheelCollider collider)
        {
            Vector3 worldScale = collider.transform.lossyScale;
            Vector3 center = collider.transform.TransformPoint(collider.center);
            float localRadius = collider.radius;
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            int pointsCount = Mathf.RoundToInt(localRadius * HitboxViewConfig.PointsPerRadius);
            float step = Mathf.PI * 2 / pointsCount;
            for (float i = 0; i <= Mathf.PI; i += step)
                positions.Add(new Vector3(center.x + radius * Mathf.Sin(i), center.y + radius * Mathf.Cos(i), center.z));
            positions.Rotate(center, collider.transform.rotation);
            SetPositions(lineRenderer, positions);
        }
        public void Initialize(TerrainCollider collider)
        {
            if (!collider.TryGetComponent<Terrain>(out Terrain terrain))
                return;
            TerrainData data = terrain.terrainData;

            Vector3 terrainSize = data.size;

            positions = data.treeInstances
                .Select(tree =>
                {
                    Vector3 normalizedPos = tree.position;
                    Vector3 localPos = new Vector3(
                        normalizedPos.x * terrainSize.x,
                        normalizedPos.y * terrainSize.y,
                        normalizedPos.z * terrainSize.z
                    );
                    return terrain.transform.TransformPoint(localPos);
                }).ToList();

            SetPositions(lineRenderer, positions);
        }
        public void Initialize(BoxCollider2D collider)
        {
            Vector3 position = collider.transform.position;
            Vector2 offset = collider.offset;
            Vector3 worldCenter = new Vector3(position.x + offset.x, position.y + offset.y, position.z);

            Vector2 half = collider.size / 2f;
            positions.AddRange([
                worldCenter + new Vector3(-half.x, -half.y, 0),
                worldCenter + new Vector3(-half.x, half.y, 0),
                worldCenter + new Vector3(half.x, half.y, 0),
                worldCenter + new Vector3(half.x, -half.y, 0),
                worldCenter + new Vector3(-half.x, -half.y, 0)
            ]);
            positions.Rotate(worldCenter, collider.transform.rotation);
            SetPositions(lineRenderer, positions);
        }
        public void Initialize(CircleCollider2D collider)
        {
            float localRadius = collider.radius;
            Vector3 worldScale = collider.transform.lossyScale;
            float radius = localRadius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            Vector2 offset = collider.offset; 
            Vector3 worldCenter = new Vector3(collider.transform.position.x + offset.x, collider.transform.position.y + offset.y, collider.transform.position.z);
            int pointsCount = Mathf.RoundToInt(collider.radius * HitboxViewConfig.PointsPerRadius);
            float step = Mathf.PI * 2 / pointsCount;
            for (float i = 0; i <= Mathf.PI; i += step)
                positions.Add(new Vector3(worldCenter.x + radius * Mathf.Sin(i), worldCenter.y + radius * Mathf.Cos(i), worldCenter.z));
            positions.Rotate(worldCenter, collider.transform.rotation);
            SetPositions(lineRenderer, positions);
        }
        public void Initialize(CapsuleCollider2D collider)
        {
        }

        public void Initialize(PolygonCollider2D collider)
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < collider.pathCount; i++)
            {
                Vector2[] path = collider.GetPath(i);
                foreach (Vector2 localPoint in path)
                {
                    Vector3 worldPoint = collider.transform.TransformPoint(localPoint);
                    sum += worldPoint;
                    positions.Add(worldPoint);
                }
            }
            positions.Rotate(sum / positions.Count, collider.transform.rotation);
            SetPositions(lineRenderer, positions);
        }
        public void Initialize(EdgeCollider2D collider)
        {

        }
        public void Initialize(CompositeCollider2D collider)
        {

        }
    }
}
