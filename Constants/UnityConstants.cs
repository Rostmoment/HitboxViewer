using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Constants
{
    public static class UnityConstants
    {
        public readonly static Vector3 NaNVector = new Vector3(float.NaN, float.NaN, float.NaN);
        public readonly static Quaternion NaNQuaternion = new Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
    }
}
