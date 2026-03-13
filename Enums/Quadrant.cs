using HitboxViewer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace HitboxViewer.Enums
{
    public static class QuadrantExtensions
    {
        extension(Quadrant quadrant)
        {

            public void GetMinMax(out float min, out float max)
            {
                min = quadrant switch
                {
                    Quadrant.First => 0f,
                    Quadrant.Second => MathConstants.HALF_PI,
                    Quadrant.Third => Mathf.PI,
                    Quadrant.Fourth => MathConstants.ONE_AND_HALF_PI,
                    _ => throw new ArgumentException($"Unknown quadrant {quadrant}")
                };

                max = quadrant switch
                {
                    Quadrant.First => MathConstants.HALF_PI,
                    Quadrant.Second => Mathf.PI,
                    Quadrant.Third => MathConstants.ONE_AND_HALF_PI,
                    Quadrant.Fourth => MathConstants.TWO_PI,
                    _ => throw new ArgumentException($"Unknown quadrant {quadrant}")
                };
            }
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
