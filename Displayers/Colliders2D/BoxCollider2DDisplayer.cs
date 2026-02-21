using UnityEngine;

namespace HitboxViewer.Displayers.Colliders2D
{
    public class BoxCollider2DDisplayer : Collider2DDisplayer<BoxCollider2D>
    {
        private static readonly Vector3[] corners = new Vector3[4];

        private Vector2 savedSize = BasePlugin.NaNVector;
        private Vector2 savedOffset = BasePlugin.NaNVector;
        private Vector3 savedPosition = BasePlugin.NaNVector;
        private Quaternion savedRotation = BasePlugin.NaNQuaternion;
        private Vector3 savedScale = BasePlugin.NaNVector;

        protected override void _Visualize()
        {
            Transform t = target.transform;

            savedSize = GenericTarget.size;
            savedOffset = GenericTarget.offset;
            savedPosition = t.position;
            savedRotation = t.rotation;
            savedScale = t.lossyScale;

            Vector2 size = GenericTarget.size * 0.5f;
            Vector2 offset = GenericTarget.offset;


            corners[0] = t.TransformPoint(offset + new Vector2(-size.x, -size.y));
            corners[1] = t.TransformPoint(offset + new Vector2(-size.x, size.y));
            corners[2] = t.TransformPoint(offset + new Vector2(size.x, size.y));
            corners[3] = t.TransformPoint(offset + new Vector2(size.x, -size.y));

            SetPositions(
                corners[0],
                corners[1],
                corners[2],
                corners[3],
                corners[0]
            );
        }

        protected override bool _ShouldBeUpdated()
        {
            return (savedSize != GenericTarget.size
                    || savedOffset != GenericTarget.offset
                    || savedPosition != target.transform.position
                    || savedRotation != target.transform.rotation
                    || savedScale != target.transform.lossyScale);
        }
    }
}