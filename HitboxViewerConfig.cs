using BepInEx;
using BepInEx.Configuration;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer
{
    class HitboxViewConfig
    {
        public static ConfigFile StaticConfig { get; private set; }
        private static readonly Dictionary<Type, string> hitboxDefaultColors = new Dictionary<Type, string>()
        {
            {typeof(BoxCollider), "#DB220D"},
            {typeof(SphereCollider), "#0D2FDB"},
            {typeof(CapsuleCollider), "#28DB0D"},
            {typeof(MeshCollider), "#DBDB0D"},
            {typeof(WheelCollider), "#DB7B0D"},
            {typeof(TerrainCollider), "#A020F0"},
            {typeof(BoxCollider2D), "#FF19AF"},
            {typeof(CircleCollider2D), "#039AFF"},
            {typeof(CapsuleCollider2D), "#633310"},
            {typeof(CharacterController), "#8A2BE2"},
            {typeof(PolygonCollider2D), "#000000"},
            {typeof(EdgeCollider2D), "#FFFFFF"},
            {typeof(CompositeCollider2D), "#363636"},
            {typeof(NavMeshObstacle), "#008080"}
        };
        private static Dictionary<Type, ConfigEntry<string>> hitboxColors = new Dictionary<Type, ConfigEntry<string>>()
        {
        };
        public static Color GetHitboxColor(object collider)
        {
            if (!hitboxColors.TryGetValue(collider.GetType(), out ConfigEntry<string> color))
                return Color.clear;
            return HelpfulMethods.ColorFromHex(color.Value);
        }

        private static ConfigEntry<int> pointsPerRadius;
        public static int PointsPerRadius
        {
            get => pointsPerRadius.Value;
            set => pointsPerRadius.Value = value;
        }

        private static ConfigEntry<float> hitboxLineWidth; 
        public static float HitboxLineWidth
        {
            get => hitboxLineWidth.Value;
            set => hitboxLineWidth.Value = value;
        }

        private static ConfigEntry<string> shaderHitboxName;
        public static string ShaderHitboxName
        {
            get => shaderHitboxName.Value;
            set => shaderHitboxName.Value = value;
        }

        private static ConfigEntry<KeyCode> changeColliderVisualizeMode;
        public static KeyCode ChangeColliderVisualizeMode
        {
            get => changeColliderVisualizeMode.Value;
            set => changeColliderVisualizeMode.Value = value;
        }
        private static ConfigEntry<KeyCode> changeNavMeshObstacleVisualizeMode;
        public static KeyCode ChangeNavMeshObstacleVisualizeMode
        {
            get => changeNavMeshObstacleVisualizeMode.Value;
            set => changeNavMeshObstacleVisualizeMode.Value = value;
        }

        private static ConfigEntry<int> updateRate;
        public static int UpdateRate
        {
            get => updateRate.Value;
            set => updateRate.Value = value;
        }

        public static void Initialize()
        {
            StaticConfig = new ConfigFile($"{Paths.ConfigPath}/{PluginInfo.GUID}.cfg", true);
            foreach (var data in hitboxDefaultColors)
            {
                hitboxColors[data.Key] = StaticConfig.Bind("Colors", $"{data.Key.Name} Color", data.Value, $"Color that will be used to display hitboxes of {data.Key.Name} type");
            }
            pointsPerRadius = StaticConfig.Bind("Visualization", "Points Per Radius", 100, "Defines how many points are used per unit of circle radius\nTotal = N × radius. Applies to all round hitboxes");
            hitboxLineWidth = StaticConfig.Bind("Visualization", "Hitbox Line Width", 0.1f, "Line width for hitbox outlines");
            shaderHitboxName = StaticConfig.Bind("Visualization", "Shader Name", "Unlit/Color", "Name of shader that will be used for coloring hitbox outlines\nAdded because not every game has this shader");

            updateRate = StaticConfig.Bind("Update", "Update Rate", 60, "Determines once every how many frames new hitbox outlines will be calculated\nIf zero or less hitboxes won't be updated themselves. You will need add and update them manually to object with UnityExplorer (HitboxView.HitboxDisplay component)");

            changeColliderVisualizeMode = StaticConfig.Bind("Key Binds", "Change Collider Visualization Mode", KeyCode.F1, "Key that is used for changing collider visualization mode");
            changeNavMeshObstacleVisualizeMode = StaticConfig.Bind("Key Binds", "Change NavMeshObstacle Visualization Mode", KeyCode.F2, "Key that is used for changing NavMeshObstacle visualization mode");
        }
    }
}
