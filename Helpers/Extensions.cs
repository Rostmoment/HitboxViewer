using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer.Helpers
{
    public static class Extensions
    {
        public static void Rotate(this List<Vector3> positions, Vector3 center, Vector3 euler) => Rotate(positions, center, Quaternion.Euler(euler));
        public static void Rotate(this List<Vector3> positions, Vector3 center, Quaternion rotation)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] = positions[i].Rotate(center, rotation);
            }
        }
        public static Vector3 Rotate(this Vector3 point, Vector3 center, Vector3 euler) => point.Rotate(center, Quaternion.Euler(euler));
        public static Vector3 Rotate(this Vector3 point, Vector3 center, Quaternion rotation) => rotation * (point - center) + center;
        public static void Repatch(this Harmony harmony, bool useTry = true)
        {
            harmony.UnpatchSelf();
            if (useTry)
                harmony.TryPatchAll();
            else
                harmony.PatchAll();
        }
        public static void TryPatchAll(this Harmony harmony, Assembly assembly)
        {
            foreach (Type type in AccessTools.GetTypesFromAssembly(assembly))
            {
                try
                {
                    harmony.CreateClassProcessor(type).Patch();
                }
                catch
                {
                }
            }
        }

        public static void TryPatchAll(this Harmony harmony)
        {
            harmony.TryPatchAll(new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly);
        }


        public static bool IfExists<T>(this IEnumerable<T> values, Func<T, bool> func, out T result)
        {
            result = default;
            if (values.EmptyOrNull())
                return false;
            IEnumerable<T> list = values.Where(func);
            if (list.Count() > 0)
                result = list.First();
            return list.Count() != 0;
        }
        public static T FindInArray<T>(this T[] values, Func<T, bool> func) => values.Where(func).FirstOrDefault();
        public static bool EmptyOrNull<T>(this IEnumerable<T> values)
        {
            if (values == null) return true;
            return values.Count() == 0;
        }
        public static bool ArrayIsEmptyOrNull<T>(this Array values)
        {
            if (values == null) return true;
            return values.Length == 0;
        }


        public static string RemoveLastN(this string str, int N)
        {
            if (N > str.Length || N == str.Length)
                return "";
            return str.Substring(0, str.Length - N);
        }
        // Some games don't support this methods, so I need recreate them
        public static T ParseToEnum<T>(this string text, bool ignoreCase = true) where T : Enum
        {
            if (text.TryParseToEnum(ignoreCase, out T res)) return res;
            throw new ArgumentException("No enum with type " + typeof(T).ToString() + " and name " + text);
        }
        public static bool TryParseToEnum<T>(this string text, out T res) where T : Enum => text.TryParseToEnum(true, out res);
        public static bool TryParseToEnum<T>(this string text, bool ignoreCase, out T res) where T : Enum
        {
            if (ignoreCase)
            {
                if (HelpfulMethods.AllOfEnum<T>().IfExists(x => x.ToString().ToLower() == text.ToLower(), out res))
                    return true;
            }
            else
            {
                if (HelpfulMethods.AllOfEnum<T>().IfExists(x => x.ToString() == text, out res))
                    return true;
            }
            res = default;
            return false;
        }
        public static T ReadEnum<T>(this BinaryReader reader) where T : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));

            object value = underlyingType switch
            {
                Type t when t == typeof(int) => reader.ReadInt32(),
                Type t when t == typeof(byte) => reader.ReadByte(),
                Type t when t == typeof(short) => reader.ReadInt16(),
                Type t when t == typeof(long) => reader.ReadInt64(),
                Type t when t == typeof(uint) => reader.ReadUInt32(),
                Type t when t == typeof(ushort) => reader.ReadUInt16(),
                Type t when t == typeof(ulong) => reader.ReadUInt64(),
                Type t when t == typeof(char) => reader.ReadChar(),
                _ => throw new InvalidOperationException($"Unsupported enum underlying type: {underlyingType}")
            };

            return (T)Enum.ToObject(typeof(T), value);
        }

        public static void Write<T>(this BinaryWriter writer, T t) where T : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(T));

            if (underlyingType == typeof(int))
                writer.Write((int)(object)t);
            else if (underlyingType == typeof(byte))
                writer.Write((byte)(object)t);
            else if (underlyingType == typeof(short))
                writer.Write((short)(object)t);
            else if (underlyingType == typeof(long))
                writer.Write((long)(object)t);
            else if (underlyingType == typeof(uint))
                writer.Write((uint)(object)t);
            else if (underlyingType == typeof(ushort))
                writer.Write((ushort)(object)t);
            else if (underlyingType == typeof(ulong))
                writer.Write((ulong)(object)t);
            else if (underlyingType == typeof(char))
                writer.Write((char)(object)t);
        }


        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            if (!obj.TryGetComponent(out T component))
                component = obj.AddComponent<T>();
            return component;
        }
        public static bool DeleteComponent<T>(this GameObject obj) where T : Component
        {
            if (obj == null) return false;
            if (obj.GetComponent<T>() == null) return false;
            UnityEngine.Object.Destroy(obj.GetComponent<T>());
            return true;
        }
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component => gameObject.TryGetComponent<T>(out _);
        public static bool TryGetComponent<T>(this GameObject gameObject, out T res) where T : Component
        {
            res = gameObject.GetComponent<T>();
            return res != null;
        }
        public static bool TryGetComponent<T>(this Component component, out T res) where T : Component
        {
            res = component.GetComponent<T>();
            return res != null;
        }

        public static Vector3[] GetVertices(this NavMeshObstacle obstacle)
        {
            Transform trans = obstacle.transform;
            Vector3 center = obstacle.center;
            Vector3 size = obstacle.size * 0.5f;
            Vector3[] localCorners = [
                new Vector3(-size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y,  size.z),
                new Vector3(-size.x,  size.y, -size.z),
                new Vector3(-size.x,  size.y,  size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(size.x, -size.y,  size.z),
                new Vector3(size.x,  size.y, -size.z),
                new Vector3(size.x,  size.y,  size.z)
            ];
            Vector3[] worldCorners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                worldCorners[i] = trans.TransformPoint(center + localCorners[i]);
            }
            return worldCorners;
        }
        public static Vector3[] GetVertices(this BoxCollider collider)
        {
            Transform trans = collider.transform;
            Vector3 center = collider.center;
            Vector3 size = collider.size * 0.5f;
            Vector3[] localCorners = [
                new Vector3(-size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y,  size.z),
                new Vector3(-size.x,  size.y, -size.z),
                new Vector3(-size.x,  size.y,  size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(size.x, -size.y,  size.z),
                new Vector3(size.x,  size.y, -size.z),
                new Vector3(size.x,  size.y,  size.z)
            ];
            Vector3[] worldCorners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                worldCorners[i] = trans.TransformPoint(center + localCorners[i]);
            }
            return worldCorners;
        }


        public static List<List<T>> SplitList<T>(this List<T> values, int chunkSize)
        {
            List<List<T>> res = new List<List<T>>();
            for (int i = 0; i < values.Count; i += chunkSize)
            {
                res.Add(values.GetRange(i, Math.Min(chunkSize, values.Count - i)));
            }
            return res;
        }

        public static K FindKey<K, V>(this Dictionary<K, V> dictionary, V value)
        {
            return dictionary.Where(x => x.Value.Equals(value)).FirstOrDefault().Key;
        }
        public static bool TryFindKey<K, V>(this Dictionary<K, V> dictonary, V value, out K key)
        {
            key = dictonary.FindKey(value);
            return key != null;
        }

        public static T Next<T>(this T value) where T : Enum
        {
            int intValue = Convert.ToInt32(value);
            int nextValue = (intValue + 1) % Enum.GetValues(typeof(T)).Length;
            return (T)Enum.ToObject(typeof(T), nextValue);
        }

    }
}
