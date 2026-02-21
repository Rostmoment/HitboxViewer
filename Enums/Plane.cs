using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Enums
{
    public static class PlaneExtensions
    {
        public const Plane DEFAULT_2D_PLANE = Plane.XY;
    }
    public enum Plane
    {
        XY,
        XZ,
        YZ
    }
}
