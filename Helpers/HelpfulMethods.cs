using HitboxViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Helpers
{
    class HelpfulMethods
    {
        private static Dictionary<string, Color> colorCache = new Dictionary<string, Color>();
        public static Color ColorFromHex(string hex)
        {
            if (!colorCache.TryGetValue(hex, out Color result))
            {
                if (hex.EmptyOrNull())
                    return new Color(0, 0, 0, 0);
                while (hex.StartsWith("#"))
                {
                    hex = hex.Substring(1);
                }
                List<List<char>> charList = hex.ToList().SplitList(2);
                if (charList.Count != 4 && charList.Count != 3)
                {
                    BasePlugin.Logger.LogWarning("HexCode " + hex + " is invalid!");
                    return new Color(0, 0, 0, 0);
                }
                List<int> values = new List<int>() { };
                foreach (List<char> chars in charList)
                {
                    if (chars.Count != 2)
                    {
                        BasePlugin.Logger.LogWarning("HexCode " + hex + " is invalid!");
                        return new Color(0, 0, 0, 0);
                    }
                    string temp = chars[0].ToString() + chars[1].ToString();
                    try
                    {
                        values.Add(Convert.ToInt16(temp, 16));
                    }
                    catch
                    {
                        BasePlugin.Logger.LogWarning("HexCode " + hex + " is invalid!");
                    }
                }
                if (values.Count == 4)
                    result = new Color(values[0] / 255f, values[1] / 255f, values[2] / 255f, values[3] / 255f);
                else
                    result = new Color(values[0] / 255f, values[1] / 255f, values[2] / 255f);
                colorCache[hex] = result;
            }
            return result;
        }
        public static Type[] GetDerivedTypes<T>() => GetDerivedTypes(typeof(T));
        public static Type[] GetDerivedTypes(Type baseType)
        {
            List<Type> result = new List<Type>();
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                result.AddRange(a.GetTypes().Where(x => x.IsSubclassOf(baseType)));
            }
            return result.ToArray();
        }

        public static T[] AllOfEnum<T>() where T : Enum => (T[])Enum.GetValues(typeof(T));
    }
}
