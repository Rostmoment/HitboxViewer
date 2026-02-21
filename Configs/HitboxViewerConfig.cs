using BepInEx.Configuration;
using HitboxViewer.Enums;
using HitboxViewer.Extensions;
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

        private static ConfigEntry<KeyboardShortcut> keyboardShortcut;
        public static KeyboardShortcut KeyboardShortcut => keyboardShortcut.Value;

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
                new(KeyCode.None, ColorExtensions.HexToColor("#DB220D"), ColorExtensions.HexToColor("#DB220D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(SphereCollider)] = new(
                nameof(SphereCollider),
                new RoundedHitboxConfig(KeyCode.None, ColorExtensions.HexToColor("#0D2FDB"), ColorExtensions.HexToColor("#0D2FDB"), RoundedHitboxAlgorithm.LatitudeLongitude),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),
            [typeof(CapsuleCollider)] = new(
                nameof(CapsuleCollider),
                new RoundedHitboxConfig(KeyCode.None, ColorExtensions.HexToColor("#28DB0D"), ColorExtensions.HexToColor("#28DB0D"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(MeshCollider)] = new(
                nameof(MeshCollider),
                new(KeyCode.None, ColorExtensions.HexToColor("#DBDB0D"), ColorExtensions.HexToColor("#DBDB0D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(WheelCollider)] = new(
                nameof(WheelCollider),
                new(KeyCode.None, ColorExtensions.HexToColor("#DB7B0D"), ColorExtensions.HexToColor("#DB7B0D")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(TerrainCollider)] = new(
                nameof(TerrainCollider),
                new(KeyCode.None, ColorExtensions.HexToColor("#A020F0"), ColorExtensions.HexToColor("#A020F0")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(BoxCollider2D)] = new(
                nameof(BoxCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#FF19AF"), ColorExtensions.HexToColor("#FF19AF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CircleCollider2D)] = new(
                nameof(CircleCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#039AFF"), ColorExtensions.HexToColor("#039AFF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CapsuleCollider2D)] = new(
                nameof(CapsuleCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#633310"), ColorExtensions.HexToColor("#633310")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CharacterController)] = new(
                nameof(CharacterController),
                new RoundedHitboxConfig(KeyCode.None, ColorExtensions.HexToColor("#8A2BE2"), ColorExtensions.HexToColor("#8A2BE2"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(PolygonCollider2D)] = new(
                nameof(PolygonCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#000000"), ColorExtensions.HexToColor("#000000")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(EdgeCollider2D)] = new(
                nameof(EdgeCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#FFFFFF"), ColorExtensions.HexToColor("#FFFFFF")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(CompositeCollider2D)] = new(
                nameof(CompositeCollider2D),
                new(KeyCode.None, ColorExtensions.HexToColor("#363636"), ColorExtensions.HexToColor("#363636")),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            ),

            [typeof(NavMeshObstacle)] = new(
                nameof(NavMeshObstacle),
                new RoundedHitboxConfig(KeyCode.None, ColorExtensions.HexToColor("#008080"), ColorExtensions.HexToColor("#008080"), RoundedHitboxAlgorithm.ThreeAxis),
                new RoundedHitboxUI(),
                HitboxesFlags.BoxNavMeshObstacle | HitboxesFlags.CapsuleNavMeshObstacle
            ),
        };
            


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

            keyboardShortcut = BasePlugin.Instance.Config.Bind<KeyboardShortcut>(
                "General",
                "Toggle Key",
                new KeyboardShortcut(KeyCode.F4),
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
