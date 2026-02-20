using HitboxViewer.Configs;
using HitboxViewer.Displayers.Helpers;
using HitboxViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Displayers.Colliders
{
    public class CapsuleColliderDisplayer : ColliderDisplayer<CapsuleCollider>
    {
        private Vector3 savedCenter = BasePlugin.NaNVector;
        private Vector3 savedScale = BasePlugin.NaNVector;
        private float savedRadius = float.NaN;
        private float savedHeight = float.NaN;


        protected override void _Visualize()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 center = target.transform.TransformPoint(GenericTarget.center);
            float radius = GenericTarget.radius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));
            float height = GenericTarget.height * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

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
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 center = target.transform.TransformPoint(GenericTarget.center);
            float radius = GenericTarget.radius;
            float height = GenericTarget.height;

            return savedRadius != radius || savedHeight != height || savedCenter != center || savedScale != worldScale;
        }
    }
}
