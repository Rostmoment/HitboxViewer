using HitboxViewer.Configs;
using HitboxViewer.Constants;
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
    class CharacterControllerDisplayer : ColliderDisplayer<CharacterController>
    {

        private Vector3 savedCenter = UnityConstants.NaNVector;
        private Vector3 savedScale = UnityConstants.NaNVector;
        private Quaternion savedRotation = Quaternion.identity;
        private float savedRadius = float.NaN;
        private float savedHeight = float.NaN;

        protected override void _Visualize()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 center = target.transform.TransformPoint(GenericTarget.center);

            float maxScale = Mathf.Max(
                Mathf.Abs(worldScale.x),
                Mathf.Abs(worldScale.y),
                Mathf.Abs(worldScale.z));

            float radius = GenericTarget.radius * maxScale;
            float height = GenericTarget.height * maxScale;

            savedCenter = center;
            savedScale = worldScale;
            savedRotation = target.transform.rotation;
            savedRadius = GenericTarget.radius;
            savedHeight = GenericTarget.height;

            RoundedHitboxConfig3D config = (RoundedHitboxConfig3D)Definition.Config;

            Vector3[] points = config.Algorithm switch
            {
                Enums.RoundedHitboxAlgorithm.LatitudeLongitude => CapsuleDisplayerHelper.DrawLatitudeLongitudeCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.Fibonacci => CapsuleDisplayerHelper.DrawFibonacciCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.ThreeAxis => CapsuleDisplayerHelper.DrawThreeAxisCapsule(center, radius, height, config.PointsPerUnit),
                Enums.RoundedHitboxAlgorithm.TwoAxis => CapsuleDisplayerHelper.DrawTwoAxisCapsule(center, radius, height, config.PointsPerUnit),
                _ => throw new ArgumentException($"Unknown algorithm {config.Algorithm}"),
            };

            points.RotatePoints(center, target.transform.rotation);

            SetPositions(points);
        }

        protected override bool _ShouldBeUpdated()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 center = target.transform.TransformPoint(GenericTarget.center);

            return savedRadius != GenericTarget.radius
                   || savedHeight != GenericTarget.height
                   || savedRotation != target.transform.rotation
                   || savedCenter != center
                   || savedScale != worldScale;
        }
    }
}
