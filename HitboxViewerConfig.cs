using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer
{
    static class HitboxViewerConfig
    {
        private const float DEFAULT_LINE_WIDTH = 0.1f;

        private static ConfigEntry<string> shaderName;
        public static string ShaderName => shaderName.Value;

        private static ConfigEntry<int> updateRate;
        public static int UpdateRate => updateRate.Value;

        private static Dictionary<Type, HitboxTypeConfig> hitboxes = new Dictionary<Type, HitboxTypeConfig>()
        {
            [typeof(BoxCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#DB220D"), HexToColor("#DB220D"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(SphereCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#0D2FDB"), HexToColor("#0D2FDB"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(CapsuleCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#28DB0D"), HexToColor("#28DB0D"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(MeshCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#DBDB0D"), HexToColor("#DBDB0D"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(WheelCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#DB7B0D"), HexToColor("#DB7B0D"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(TerrainCollider)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#A020F0"), HexToColor("#A020F0"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(BoxCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#FF19AF"), HexToColor("#FF19AF"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(CircleCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#039AFF"), HexToColor("#039AFF"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(CapsuleCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#633310"), HexToColor("#633310"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(CharacterController)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#8A2BE2"), HexToColor("#8A2BE2"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(PolygonCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#000000"), HexToColor("#000000"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(EdgeCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#FFFFFF"), HexToColor("#FFFFFF"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(CompositeCollider2D)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#363636"), HexToColor("#363636"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.Trigger | HitboxesFlags.NotTrigger),

            [typeof(NavMeshObstacle)] = new HitboxTypeConfig(KeyCode.None, HexToColor("#008080"), HexToColor("#008080"), DEFAULT_LINE_WIDTH, DEFAULT_LINE_WIDTH, HitboxesFlags.SphereBoxNavMeshObstacle | HitboxesFlags.BoxNavMeshObstacle),
        };

        private static Color HexToColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;

            throw new ArgumentException($"Invalid hex color string: {hex}");
        }

        public static HitboxTypeConfig InfoOf<T>() => InfoOf(typeof(T));
        public static HitboxTypeConfig InfoOf(Type type) => hitboxes[type];


        public static void Initialize()
        {
            shaderName = BasePlugin.Instance.Config.Bind<string>(
                "General",
                "Shader Name",
                "Unlit/Color",
                "Name of shader that will be used for coloring hitbox outlines\nAdded because not every game has default shader"
            );

            updateRate = BasePlugin.Instance.Config.Bind<int>(
                "General",
                "Update Rate",
                60,
                "Number of frames between each hitbox update"
            );

            foreach (var kvp in hitboxes)
                kvp.Value.Initialize(kvp.Key.Name);
        }
    }
}
