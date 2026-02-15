using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Extensions
{
    public static class ArrayExtensions
    {
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
