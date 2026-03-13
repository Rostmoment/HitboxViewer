using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using UnityEngine;

namespace HitboxViewer.Extensions
{
    public static class ArrayExtensions
    {
        extension(Vector3[] points)
        {
            public void RotatePoints(Vector3 center, Vector3 euler) => points.RotatePoints(center, Quaternion.Euler(euler));
            public void RotatePoints(Vector3 center, Quaternion rotation)
            {
                if (points == null)
                    return;

                for (int i = 0; i < points.Length; i++)
                    points[i] = rotation * (points[i] - center) + center;
            }
        }

        public static T[] Merge<T>(params T[][] arrays)
        {
            T[] result = new T[arrays.Sum(x => x.Length)];
            int index = 0;
            foreach (T[] array in arrays)
            {
                if (array == null)
                    continue;

                Array.Copy(array, 0, result, index, array.Length);
                index += array.Length;
            }

            return result;
        }

    }
}
