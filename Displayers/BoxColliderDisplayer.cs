using UnityEngine;

namespace HitboxViewer.Displayers
{
    public class BoxColliderDisplayer : BaseDisplayer<BoxCollider>
    {
        protected override void _Visualize()
        {
            Vector3 center = target.center;
            Vector3 size = target.size * 0.5f;

            Vector3[] localCorners = [
               new Vector3(-size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y,  size.z),
                new Vector3(-size.x,  size.y, -size.z),
                new Vector3(-size.x,  size.y,  size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(size.x, -size.y,  size.z),
                new Vector3(size.x,  size.y, -size.z),
                new Vector3(size.x,  size.y,  size.z)
            ];

            Vector3[] worldCorners = new Vector3[8];
            for (int i = 0; i < 8; i++)
                worldCorners[i] = target.transform.TransformPoint(center + localCorners[i]);

            SetPositions(worldCorners[0], worldCorners[1], worldCorners[5], worldCorners[4], worldCorners[0], worldCorners[2], worldCorners[3], worldCorners[7],
                worldCorners[5], worldCorners[4], worldCorners[6], worldCorners[7], worldCorners[6], worldCorners[2], worldCorners[3], worldCorners[1]);
        }
    }
}
