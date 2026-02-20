using BepInEx.Configuration;
using HitboxViewer.Enums;
using HitboxViewer.Flags;
using HitboxViewer.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer.Configs
{
    static class HitboxViewerConfig
    {
        #region general configs

        private static ConfigEntry<float> menuAlpha;
        public static float MenuAlpha => menuAlpha.Value;

        private static ConfigEntry<KeyCode> keyCode;
        public static KeyCode KeyCode => keyCode.Value;

        private static ConfigEntry<float> startupDelay;
        public static float StartupDelay => startupDelay.Value;

        private static ConfigEntry<string> shaderName;
        public static string ShaderName => shaderName.Value;

        private static ConfigEntry<int> updateRate;
        public static int UpdateRate => updateRate.Value;

        private static ConfigEntry<bool> hideOnStart;
        public static bool HideOnStart => hideOnStart.Value;
        #endregion

        private static Dictionary<Type, HitboxDefinition> hitboxes = new Dictionary<Type, HitboxDefinition>()
        {
            [typeof(BoxCollider)] = new(
                nameof(BoxCollider),
                new(KeyCode.None, HexToColor("#DB220D"), HexToColor("#DB220D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(SphereCollider)] = new(
                nameof(SphereCollider),
                new RoundedHitboxConfig(KeyCode.None, HexToColor("#0D2FDB"), HexToColor("#0D2FDB"), RoundedHitboxAlgorithm.LatitudeLongitude),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),
            [typeof(CapsuleCollider)] = new(
                nameof(CapsuleCollider),
                new RoundedHitboxConfig(KeyCode.None, HexToColor("#28DB0D"), HexToColor("#28DB0D"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(MeshCollider)] = new(
                nameof(MeshCollider),
                new(KeyCode.None, HexToColor("#DBDB0D"), HexToColor("#DBDB0D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(WheelCollider)] = new(
                nameof(WheelCollider),
                new(KeyCode.None, HexToColor("#DB7B0D"), HexToColor("#DB7B0D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(TerrainCollider)] = new(
                nameof(TerrainCollider),
                new(KeyCode.None, HexToColor("#A020F0"), HexToColor("#A020F0")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(BoxCollider2D)] = new(
                nameof(BoxCollider2D),
                new(KeyCode.None, HexToColor("#FF19AF"), HexToColor("#FF19AF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CircleCollider2D)] = new(
                nameof(CircleCollider2D),
                new(KeyCode.None, HexToColor("#039AFF"), HexToColor("#039AFF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CapsuleCollider2D)] = new(
                nameof(CapsuleCollider2D),
                new(KeyCode.None, HexToColor("#633310"), HexToColor("#633310")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CharacterController)] = new(
                nameof(CharacterController),
                new RoundedHitboxConfig(KeyCode.None, HexToColor("#8A2BE2"), HexToColor("#8A2BE2"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(PolygonCollider2D)] = new(
                nameof(PolygonCollider2D),
                new(KeyCode.None, HexToColor("#000000"), HexToColor("#000000")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(EdgeCollider2D)] = new(
                nameof(EdgeCollider2D),
                new(KeyCode.None, HexToColor("#FFFFFF"), HexToColor("#FFFFFF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CompositeCollider2D)] = new(
                nameof(CompositeCollider2D),
                new(KeyCode.None, HexToColor("#363636"), HexToColor("#363636")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(NavMeshObstacle)] = new(
                nameof(NavMeshObstacle),
                new RoundedHitboxConfig(KeyCode.None, HexToColor("#008080"), HexToColor("#008080"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.BoxNavMeshObstacle | HitboxesFlags.CapsuleNavMeshObstacle
            ),
        };
            

        private static Color HexToColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;

            throw new ArgumentException($"Invalid hex color string: {hex}");
        }

        public static HitboxDefinition DefinitionOf<T>() => DefinitionOf(typeof(T));
        public static HitboxDefinition DefinitionOf(Type type) => hitboxes[type];


        public static void Initialize()
        {
            menuAlpha = BasePlugin.Instance.Config.Bind<float>(
              "General",
              "Menu Alpha",
              1f,
              "Value of alpha channel for menu"
            );
            menuAlpha.SettingChanged += (x, y) =>
            {

                MainUI.Alpha = menuAlpha.Value;
            };
            menuAlpha.Value = Mathf.Clamp01(menuAlpha.Value);

            keyCode = BasePlugin.Instance.Config.Bind<KeyCode>(
                "General",
                "Toggle Key",
                KeyCode.F4,
                "Toggle for opening menu"
            );


            startupDelay = BasePlugin.Instance.Config.Bind<float>(
                "General",
                "Startup Delay",
                1f,
                "Number of seconds to wait before initializing hitbox viewer. Adjust it if you experience issues"
            );

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

            hideOnStart = BasePlugin.Instance.Config.Bind<bool>(
                "General", 
                "Hide On Startup",
                true,
                "Hide menu on start"
            );

            foreach (var kvp in hitboxes)
                kvp.Value.Config.Initialize();
        }
    }
}
