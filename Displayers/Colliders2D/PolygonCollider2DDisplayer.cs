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
        private Vector2[] savedPoints = null;

        protected override void _Visualize()
        {
            Transform t = target.transform;

            savedPosition = t.position;
            savedRotation = t.rotation;
            savedScale = t.lossyScale;
            savedPathCount = GenericTarget.pathCount;


            int totalPoints = 0;
            for (int i = 0; i < GenericTarget.pathCount; i++)
                totalPoints += GenericTarget.GetPath(i).Length;

            savedPoints = new Vector2[totalPoints];

            int index = 0;
            for (int i = 0; i < GenericTarget.pathCount; i++)
            {
                Vector2[] path = GenericTarget.GetPath(i);
                for (int j = 0; j < path.Length; j++)
                {
                    savedPoints[index++] = path[j];
                }
            }


            for (int i = 0; i < GenericTarget.pathCount; i++)
            {
                Vector2[] path = GenericTarget.GetPath(i);

                if (path.Length < 2)
                    continue;

                Vector3[] worldPoints = new Vector3[path.Length + 1];

                for (int j = 0; j < path.Length; j++)
                {
                    worldPoints[j] = t.TransformPoint(path[j]);
                }

                worldPoints[path.Length] = worldPoints[0];

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

            for (int i = 0; i < GenericTarget.pathCount; i++)
            {
                Vector2[] path = GenericTarget.GetPath(i);

                for (int j = 0; j < path.Length; j++)
                {
                    if (savedPoints == null || index >= savedPoints.Length || savedPoints[index] != path[j])
                        return true;

                    index++;
                }
            }

            return false;
        }
    }
}