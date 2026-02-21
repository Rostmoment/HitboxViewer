using HitboxViewer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Enums
{
    public static class QuadrantExtensions
    {
        public static (float min, float max) GetMinMax(this Quadrant quadrant)
        {
            return quadrant switch
            {
                Quadrant.First => (0f, MathConstants.HALF_PI),
                Quadrant.Second => (MathConstants.HALF_PI, Mathf.PI),
                Quadrant.Third => (Mathf.PI, MathConstants.ONE_AND_HALF_PI),
                Quadrant.Fourth => (MathConstants.ONE_AND_HALF_PI, MathConstants.TWO_PI),
                _ => throw new ArgumentOutOfRangeException(nameof(quadrant))
            };
        }

        public static void GetMinMax(this Quadrant quadrant, out float min, out float max)
        {
            (min, max) = quadrant.GetMinMax();
        }
    }
    public enum Quadrant
    {
        First,
        Second,
        Third,
        Fourth
    }
}
