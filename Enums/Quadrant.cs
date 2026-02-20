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
        private static readonly Dictionary<Quadrant, Vector2> minMax = new Dictionary<Quadrant, Vector2>()
        {
            [Quadrant.First] = new Vector2(0, MathConstants.HALF_PI),
            [Quadrant.Second] = new Vector2(MathConstants.HALF_PI, Mathf.PI),
            [Quadrant.Third] = new Vector2(Mathf.PI, MathConstants.ONE_AND_HALF_PI),
            [Quadrant.Fourth] = new Vector2(MathConstants.ONE_AND_HALF_PI, MathConstants.TWO_PI),
        };
        public static (float min, float max) GetMinMax(this Quadrant quadrant)
        {
            Vector2 vector = minMax[quadrant];
            return (vector.x, vector.y);
        }
        public static void GetMinMax(this Quadrant quadrant, out float min, out float max)
        {
            var data = quadrant.GetMinMax();
            min = data.min;
            max = data.max;
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
