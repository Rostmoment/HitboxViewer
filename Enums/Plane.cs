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
        private static Dictionary<Plane, Vector3> vectors = new Dictionary<Plane, Vector3>()
        {
            [Plane.XY] = new Vector3(1, 1, 0),
            [Plane.XZ] = new Vector3(1, 0, 1),
            [Plane.YZ] = new Vector3(0, 1, 1)
        };

        public static Vector3 GetVector(this Plane plane) => vectors[plane];
    }
    public enum Plane
    {
        XY,
        XZ,
        YZ
    }
}
