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
        protected override void _Visualize()
        {
            Vector3 worldScale = target.transform.lossyScale;
            Vector3 worldCenter = target.transform.TransformPoint(GenericTarget.center);
            float worldRadius = GenericTarget.radius * Mathf.Max(Mathf.Abs(worldScale.x), Mathf.Abs(worldScale.y), Mathf.Abs(worldScale.z));

            SetPositions(DisplayersHelper.DrawFibonacciSphere(worldCenter, worldRadius));
        }
    }
}
