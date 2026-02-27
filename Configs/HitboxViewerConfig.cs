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

            foreach (var kvp in HitboxDefinition.definitions)
                kvp.Value.Config.Initialize();
        }
    }
}
