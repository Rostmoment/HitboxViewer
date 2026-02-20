using HitboxViewer.Configs;
using HitboxViewer.Displayers.Helpers;
using HitboxViewer.Extensions;
using HitboxViewer.Flags;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer.Displayers
{
    class NavMeshObstacleDisplayer : BaseDisplayer<NavMeshObstacle>
    {
        private static Vector3[] boxVertices = new Vector3[8];

        private Vector3 savedCenter = BasePlugin.NaNVector;
        private Vector3 savedSize = BasePlugin.NaNVector;
        private Vector3 savedPosition = BasePlugin.NaNVector;
        private Quaternion savedRotation = BasePlugin.NaNQuaternion;
        private Vector3 savedScale = BasePlugin.NaNVector;
        private float savedRadius = float.NaN;
        private float savedHeight = float.NaN;

        public override HitboxesFlags HitboxFlags
        {
            get
            {
                if (GenericTarget.shape == NavMeshObstacleShape.Box)
                    return HitboxesFlags.BoxNavMeshObstacle;
                return HitboxesFlags.CapsuleNavMeshObstacle;
            }
        }

        protected override void _Visualize()
        {
            if (GenericTarget.shape == NavMeshObstacleShape.Box)
            {
                VisualizeBox();
                return;
            }
            VisualizeCapsule();
        }

        private void VisualizeBox()
        {
            savedCenter = GenericTarget.center;
            savedSize = GenericTarget.size;
            savedPosition = target.transform.position;
            savedRotation = target.transform.rotation;
            savedScale = target.transform.lossyScale;

            Vector3 center = GenericTarget.center;
            Vector3 size = GenericTarget.size * 0.5f;

            boxVertices[0] = target.transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z));
            boxVertices[1] = target.transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z));
            boxVertices[2] = target.transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z));
            boxVertices[3] = target.transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z));
            boxVertices[4] = target.transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z));
            boxVertices[5] = target.transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z));
            boxVertices[6] = target.transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z));
            boxVertices[7] = target.transform.TransformPoint(center + new Vector3(size.x, size.y, size.z));

            SetPositions(boxVertices[0], boxVertices[1], boxVertices[5], boxVertices[4], boxVertices[0], boxVertices[2], boxVertices[3], boxVertices[7],
                boxVertices[5], boxVertices[4], boxVertices[6], boxVertices[7], boxVertices[6], boxVertices[2], boxVertices[3], boxVertices[1]);
        }

        private void VisualizeCapsule()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 center = target.transform.TransformPoint(GenericTarget.center);
            float radius = GenericTarget.radius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = GenericTarget.height * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            savedCenter = center;
            savedScale = worldScale;
            savedRadius = GenericTarget.radius;
            savedHeight = GenericTarget.height;

            RoundedHitboxConfig config = (RoundedHitboxConfig)Definition.Config;

            Vector3[] points = config.Algorithm switch
            {
                Enums.RoundedHitboxAlgorithm.LatitudeLongitude => CapsuleDisplayerHelper.DrawLatitudeLongitudeCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.Fibonacci => CapsuleDisplayerHelper.DrawFibonacciCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.ThreeAxis => CapsuleDisplayerHelper.DrawThreeAxisCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.TwoAxis => CapsuleDisplayerHelper.DrawTwoAxisCapsule(center, radius, height, config.PointsPerUnit),
                _ => throw new ArgumentException($"Unknown algorithm {config.Algorithm}"),
            };
            points.RotatePoints(center, transform.rotation);

            SetPositions(points);
        }

        public override bool _ShouldBeUpdated()
        {
            if (GenericTarget.shape == NavMeshObstacleShape.Box)
            {
                return savedCenter != GenericTarget.center || savedSize != GenericTarget.size || savedPosition != target.transform.position ||
                       savedRotation != target.transform.rotation || savedScale != target.transform.lossyScale;
            }
            else
            {
                Vector3 worldScale = target.transform.lossyScale;
                Vector3 center = target.transform.TransformPoint(GenericTarget.center);
                return savedRadius != GenericTarget.radius || savedHeight != GenericTarget.height || savedCenter != center || savedScale != worldScale;
            }
        }
    }
}