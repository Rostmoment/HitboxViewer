using UnityEngine;

namespace HitboxViewer.Displayers
{
    public class BoxColliderDisplayer : ColliderDisplayer<BoxCollider>
    {
        private static readonly Vector3[] corners = new Vector3[8];

        private Vector3 savedCenter = BasePlugin.NaNVector;
        private Vector3 savedSize = BasePlugin.NaNVector;
        private Vector3 savedPosition = BasePlugin.NaNVector;
        private Quaternion savedRotation = BasePlugin.NaNQuaternion;
        private Vector3 savedScale = BasePlugin.NaNVector;

        protected override void _Visualize()
        {
            savedSize = target.size;
            savedCenter = target.center;
            savedPosition = target.transform.position;
            savedRotation = target.transform.rotation;
            savedScale = target.transform.lossyScale;

            Vector3 center = target.center;
            Vector3 size = target.size * 0.5f;

            corners[0] = target.transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z));
            corners[1] = target.transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z));
            corners[2] = target.transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z));
            corners[3] = target.transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z));
            corners[4] = target.transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z));
            corners[5] = target.transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z));
            corners[6] = target.transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z));
            corners[7] = target.transform.TransformPoint(center + new Vector3(size.x, size.y, size.z));

            SetPositions(corners[0], corners[1], corners[5], corners[4], corners[0], corners[2], corners[3], corners[7],
                corners[5], corners[4], corners[6], corners[7], corners[6], corners[2], corners[3], corners[1]);
        }

        public override bool _ShouldBeUpdated()
        {
            return (savedCenter != target.center || savedSize != target.size || savedPosition != target.transform.position ||
                   savedRotation != target.transform.rotation || savedScale != target.transform.lossyScale) && base._ShouldBeUpdated();
        }
    }
}
