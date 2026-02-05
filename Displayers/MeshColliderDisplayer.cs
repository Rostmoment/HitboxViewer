using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace HitboxViewer.Displayers
{
    class MeshColliderDisplayer : ColliderDisplayer<MeshCollider>
    {
        protected override void _Visualize()
        {
            if (target.sharedMesh == null)
                return;

            Mesh mesh = target.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            HashSet<(int, int)> uniqueEdges = new HashSet<(int, int)>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int i0 = triangles[i];
                int i1 = triangles[i + 1];
                int i2 = triangles[i + 2];

                AddEdge(i0, i1);
                AddEdge(i1, i2);
                AddEdge(i2, i0);
            }

            int edgeCount = uniqueEdges.Count;
            Vector3[] positions = new Vector3[edgeCount * 2 + 1];
            int index = 0;

            foreach (var edge in uniqueEdges)
            {
                Vector3 worldA = target.transform.TransformPoint(vertices[edge.Item1]);
                Vector3 worldB = target.transform.TransformPoint(vertices[edge.Item2]);

                positions[index++] = worldA;
                positions[index++] = worldB;
            }

            positions[index] = positions[0];

            SetPositions(positions);

            void AddEdge(int a, int b)
            {
                (int, int) edge = (Mathf.Min(a, b), Mathf.Max(a, b));
                uniqueEdges.Add(edge);
            }
        }
    }
}