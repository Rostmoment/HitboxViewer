using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Extensions
{
    public static class ColorExtensions
    {
        public static Color HexToColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;

            throw new ArgumentException($"Invalid hex color string: {hex}");
        }
        public static string ToRGBAHex(this Color color) => ColorUtility.ToHtmlStringRGBA(color);
        public static string ToRGBHex(this Color color) => ColorUtility.ToHtmlStringRGB(color);
    }
}
