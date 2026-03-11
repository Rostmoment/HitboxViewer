using HitboxViewer.Constants;
using UnityEngine;

namespace HitboxViewer.Displayers.Colliders2D
{
    public class PolygonCollider2DDisplayer : Collider2DDisplayer<PolygonCollider2D>
    {
        private Vector3 savedPosition = UnityConstants.NaNVector;
        private Quaternion savedRotation = UnityConstants.NaNQuaternion;
        private Vector3 savedScale = UnityConstants.NaNVector;
        private int savedPathCount = -1;

        protected override void _Visualize()
        {
            Transform t = target.transform;
            savedPosition = t.position;
            savedRotation = t.rotation;
            savedScale = t.lossyScale;

            int pathCount = GenericTarget.pathCount;
            savedPathCount = pathCount;

            Vector2[][] paths = new Vector2[pathCount][];
            for (int i = 0; i < pathCount; i++)
                paths[i] = GenericTarget.GetPath(i);

            for (int i = 0; i < pathCount; i++)
            {
                Vector2[] path = paths[i];
                int pathLen = path.Length;
                if (pathLen < 2) continue;

                Vector3[] worldPoints = new Vector3[pathLen + 1];
                for (int j = 0; j < pathLen; j++)
                    worldPoints[j] = t.TransformPoint(path[j]);

                worldPoints[pathLen] = worldPoints[0];
                SetPositions(worldPoints);
            }
        }

        protected override bool _ShouldBeUpdated()
        {
            Transform t = target.transform;
            if (savedPosition != t.position
                || savedRotation != t.rotation
                || savedScale != t.lossyScale
                || savedPathCount != GenericTarget.pathCount)
                return true;

            int index = 0;
            int pathCount = GenericTarget.pathCount;
            for (int i = 0; i < pathCount; i++)
            {
                Vector2[] path = GenericTarget.GetPath(i);
                int pathLen = path.Length;
                if (pathLen < 2) continue;

                // +1 because SetPositions appends closing point
                if (index + pathLen + 1 > points.Length)
                    return true;

                for (int j = 0; j < pathLen; j++)
                {
                    if ((Vector2)points[index + j] != path[j])
                        return true;
                }
                index += pathLen + 1;
            }
            return false;
        }
    }
}