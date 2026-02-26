using HitboxViewer.Constants;
using UnityEngine;
using UniverseLib.Utility;

namespace HitboxViewer.Displayers.Colliders
{
    public class BoxColliderDisplayer : ColliderDisplayer<BoxCollider>
    {
        private static readonly Vector3[] corners = new Vector3[8];

        private Vector3 savedCenter = UnityConstants.NaNVector;
        private Vector3 savedSize = UnityConstants.NaNVector;
        private Vector3 savedPosition = UnityConstants.NaNVector;
        private Quaternion savedRotation = UnityConstants.NaNQuaternion;
        private Vector3 savedScale = UnityConstants.NaNVector;

        protected override void _Visualize()
        {
            Transform t = target.transform;

            savedSize = GenericTarget.size;
            savedCenter = GenericTarget.center;
            savedPosition = t.position;
            savedRotation = t.rotation;
            savedScale = t.lossyScale;

            Vector3 center = GenericTarget.center;
            Vector3 size = GenericTarget.size * 0.5f;

            // TransformPoint is matrix multiplication, so it's fast and includes rotation, scale, position, etc
            corners[0] = t.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z));
            corners[1] = t.TransformPoint(center + new Vector3(-size.x, -size.y, size.z));
            corners[2] = t.TransformPoint(center + new Vector3(-size.x, size.y, -size.z));
            corners[3] = t.TransformPoint(center + new Vector3(-size.x, size.y, size.z));
            corners[4] = t.TransformPoint(center + new Vector3(size.x, -size.y, -size.z));
            corners[5] = t.TransformPoint(center + new Vector3(size.x, -size.y, size.z));
            corners[6] = t.TransformPoint(center + new Vector3(size.x, size.y, -size.z));
            corners[7] = t.TransformPoint(center + new Vector3(size.x, size.y, size.z));

            // Order of vertices in array is fixed, so I can just connect them, doing it in less than 12 steps is impossible
            SetPositions(corners[0], corners[1], corners[5], corners[4], corners[0], corners[2], corners[3], corners[7],
                corners[5], corners[4], corners[6], corners[7], corners[6], corners[2], corners[3], corners[1]);
        }

        protected override bool _ShouldBeUpdated()
        {
            return (savedCenter != GenericTarget.center || savedSize != GenericTarget.size || savedPosition != target.transform.position ||
                   savedRotation != target.transform.rotation || savedScale != target.transform.lossyScale) && base._ShouldBeUpdated();
        }
    }
}
