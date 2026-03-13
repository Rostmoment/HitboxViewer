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
        extension(Color)
        {
            public static Color HexToColor(string hex)
            {
                if (ColorUtility.TryParseHtmlString(hex, out var color))
                    return color;

                throw new ArgumentException($"Invalid hex color string: {hex}");
            }
        }
        extension(Color color)
        {
            public string ToRGBAHex() => ColorUtility.ToHtmlStringRGBA(color);
            public string ToRGBHex() => ColorUtility.ToHtmlStringRGB(color);
        }
    }
}
