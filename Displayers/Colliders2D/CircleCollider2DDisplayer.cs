using HitboxViewer.Configs;
using HitboxViewer.Constants;
using HitboxViewer.Displayers.Helpers;
using HitboxViewer.Enums;
using HitboxViewer.Extensions;
using UnityEngine;

namespace HitboxViewer.Displayers.Colliders2D
{
    public class CircleCollider2DDisplayer : Collider2DDisplayer<CircleCollider2D>
    {
        private Vector3 savedCenter = UnityConstants.NaNVector;
        private Vector3 savedScale = UnityConstants.NaNVector;
        private Quaternion savedRotation = UnityConstants.NaNQuaternion;
        private float savedRadius = float.NaN;

        protected override void _Visualize()
        {
            Transform t = target.transform;

            Vector3 worldScale = t.lossyScale;

            float maxScale = Mathf.Max(
                Mathf.Abs(worldScale.x),
                Mathf.Abs(worldScale.y));

            float worldRadius = GenericTarget.radius * maxScale;

            Vector3 worldCenter = t.TransformPoint(GenericTarget.offset);

            savedCenter = worldCenter;
            savedScale = worldScale;
            savedRotation = t.rotation;
            savedRadius = GenericTarget.radius;

            RoundedHitboxConfig config = (RoundedHitboxConfig)Definition.Config;

            Vector3[] points = CircleDisplayerHelper.DrawCircle(worldCenter, worldRadius, PlaneExtensions.DEFAULT_2D_PLANE, config.PointsPerUnit);

            points.RotatePoints(worldCenter, t.rotation);

            SetPositions(points);
        }

        protected override bool _ShouldBeUpdated()
        {
            Transform t = target.transform;

            Vector3 worldScale = t.lossyScale;
            Vector3 worldCenter = t.TransformPoint(GenericTarget.offset);

            return savedRadius != GenericTarget.radius
                   || savedRotation != t.rotation
                   || savedCenter != worldCenter
                   || savedScale != worldScale;
        }
    }
}