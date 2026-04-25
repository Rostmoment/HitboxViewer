using HitboxViewer.Constants;
using HitboxViewer.Flags;
using UnityEngine;

namespace HitboxViewer.Displayers
{
    public class RectTransformDisplayer : BaseDisplayer<RectTransform>
    {
        private static readonly Vector3[] corners = new Vector3[4];

        private Vector2 savedSizeDelta = UnityConstants.NaNVector;
        private Vector2 savedAnchorMin = UnityConstants.NaNVector;
        private Vector2 savedAnchorMax = UnityConstants.NaNVector;
        private Vector2 savedPivot = UnityConstants.NaNVector;
        private Vector3 savedPosition = UnityConstants.NaNVector;
        private Quaternion savedRotation = UnityConstants.NaNQuaternion;
        private Vector3 savedScale = UnityConstants.NaNVector;

        public override HitboxesFlags HitboxFlags => HitboxesFlags.NotTrigger;

        protected override void _Visualize()
        {
            Transform t = GenericTarget.transform;

            savedSizeDelta = GenericTarget.sizeDelta;
            savedAnchorMin = GenericTarget.anchorMin;
            savedAnchorMax = GenericTarget.anchorMax;
            savedPivot = GenericTarget.pivot;
            savedPosition = t.position;
            savedRotation = t.rotation;
            savedScale = t.lossyScale;

            // GetWorldCorners fills: [0]=bottom-left, [1]=top-left, [2]=top-right, [3]=bottom-right
            GenericTarget.GetWorldCorners(corners);

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
            Transform t = GenericTarget.transform;

            return savedSizeDelta != GenericTarget.sizeDelta
                || savedAnchorMin != GenericTarget.anchorMin
                || savedAnchorMax != GenericTarget.anchorMax
                || savedPivot != GenericTarget.pivot
                || savedPosition != t.position
                || savedRotation != t.rotation
                || savedScale != t.lossyScale;
        }
    }
}