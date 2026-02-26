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
        // Using NaN as an "uninitialized" marker
        // Any comparison with NaN returns false and Nan != x always returns true, so the first _ShouldBeUpdated() call
        // Will always trigger visualization without needing an extra bool flag
        public readonly static Vector3 NaNVector = new Vector3(float.NaN, float.NaN, float.NaN);
        public readonly static Quaternion NaNQuaternion = new Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
    }
}
