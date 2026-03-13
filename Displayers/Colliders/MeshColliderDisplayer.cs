using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitboxViewer.Displayers.Colliders
{
    class MeshColliderDisplayer : ColliderDisplayer<MeshCollider>
    {
        struct Edge(int a, int b) : IEquatable<Edge>
        {
            public int A = Mathf.Min(a, b), B = Mathf.Max(a, b);

            public bool Equals(Edge other) => A == other.A && B == other.B;
            public override int GetHashCode() => A * 397 ^ B;
        }

        protected override void _Visualize()
        {
            if (GenericTarget.sharedMesh == null)
                return;

            Mesh mesh = GenericTarget.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            HashSet<Edge> uniqueEdges = new HashSet<Edge>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                uniqueEdges.Add(new Edge(triangles[i], triangles[i + 1]));
                uniqueEdges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
                uniqueEdges.Add(new Edge(triangles[i + 2], triangles[i]));
            }

            Vector3[] positions = new Vector3[uniqueEdges.Count * 2 + 1];
            int index = 0;

            foreach (Edge edge in uniqueEdges)
            {
                positions[index++] = target.transform.TransformPoint(vertices[edge.A]);
                positions[index++] = target.transform.TransformPoint(vertices[edge.B]);
            }

            positions[index] = positions[0];
            SetPositions(positions);
        }
    }
}