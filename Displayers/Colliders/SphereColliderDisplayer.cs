using HitboxViewer.Configs;
using HitboxViewer.Displayers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Displayers.Colliders
{
    class SphereColliderDisplayer : ColliderDisplayer<SphereCollider>
    {
        private Vector3 savedCenter = BasePlugin.NaNVector;
        private Vector3 savedScale = BasePlugin.NaNVector;
        private float savedRadius = float.NaN;

        protected override void _Visualize()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 worldCenter = target.transform.TransformPoint(GenericTarget.center);
            float worldRadius = GenericTarget.radius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            savedCenter = worldCenter; // transform.position is stored there, so no point of storing position as other variable 
            savedScale = worldScale;
            savedRadius = GenericTarget.radius;

            RoundedHitboxConfig config = (RoundedHitboxConfig)HitboxViewerConfig.DefinitionOf<SphereCollider>().Config;

            Vector3[] points;

            switch (config.Algorithm)
            {
                case Enums.RoundedHitboxAlgorithm.LatitudeLongitude:
                    points = SphereDisplayerHelper.DrawLatitudeLongitudeSphere(worldCenter, worldRadius, config.PointsPerUnit);
                    break;
                case Enums.RoundedHitboxAlgorithm.Fibonacci:
                    points = SphereDisplayerHelper.DrawFibonacciSphere(worldCenter, worldRadius, config.PointsPerUnit);
                    break;
                case Enums.RoundedHitboxAlgorithm.ThreeAxis:
                    points = SphereDisplayerHelper.DrawThreeAxisSphere(worldCenter, worldRadius, config.PointsPerUnit);
                    break;
                case Enums.RoundedHitboxAlgorithm.TwoAxis:
                    points = SphereDisplayerHelper.DrawTwoAxisSphere(worldCenter, worldRadius, config.PointsPerUnit);
                    break;
                default:
                    throw new ArgumentException($"Unknown algorithm {config.Algorithm}");
            }

            SetPositions(points);
        }
        public override bool _ShouldBeUpdated()
        {
            return savedRadius != GenericTarget.radius || savedCenter != target.transform.TransformPoint(GenericTarget.center) || savedScale != target.transform.lossyScale;
        }
    }
}
