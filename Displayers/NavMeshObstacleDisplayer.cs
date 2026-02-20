using HitboxViewer.Displayers.Helpers;
using HitboxViewer.Extensions;
using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer.Displayers
{
    class NavMeshObstacleDisplayer : BaseDisplayer<NavMeshObstacle>
    {
        private static Vector3[] boxVertices = new Vector3[8];

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

            Vector3[] points = CapsuleDisplayerHelper.DrawTwoAxisCapsule(center, radius, height);
            points.RotatePoints(center, transform.rotation);

            SetPositions(points);
        }
    }
}
