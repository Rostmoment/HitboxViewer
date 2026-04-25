using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace HitboxViewer.Extensions
{
    public static class CapsuleColliderExtensions
    {
        extension(CapsuleCollider collider)
        {
            public Quaternion DirectionRotation
            {
                get
                {
                    return collider.direction switch
                    {
                        0 => Quaternion.Euler(0f, 0f, 90f),   // X-axis
                        1 => Quaternion.identity,             // Y-axis
                        2 => Quaternion.Euler(90f, 0f, 0f),   // Z-axis
                        _ => Quaternion.identity
                    };
                }
            }
            public Quaternion ActualRotation => collider.transform.rotation * collider.DirectionRotation;
        }
    }
}
