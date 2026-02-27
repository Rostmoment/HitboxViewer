using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HitboxViewer.Configs;
using HitboxViewer.Displayers;
using HitboxViewer.Displayers.Colliders;
using HitboxViewer.Enums;
using HitboxViewer.Extensions;
using HitboxViewer.Flags;
using HitboxViewer.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer
{
    class PluginInfo
    {
        public const string GUID = "rost.moment.unity.hitboxviewer";
        public const string VERSION = "0.1.0";
        public const string NAME = "Hitbox Viewer";
    }
   

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { private set; get; }
        public static new ManualLogSource Logger { private set; get; }
        public static GameObject MainObject { private set; get; }


        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            DefineHitboxes();

            MainObject = new GameObject("HitboxViewerMainObject");
            MainObject.AddComponent<HitboxUpdater>();
            DontDestroyOnLoad(MainObject);

            HitboxViewerConfig.Initialize();

            UniverseLib.Universe.Init(HitboxViewerConfig.StartupDelay, MainUI.InitializeUI, (x, y) => { }, new UniverseLib.Config.UniverseLibConfig()
            {
                Force_Unlock_Mouse = true,
            });
        }


        private void Update()
        {
            if (HitboxViewerConfig.KeyboardShortcut.IsDown())
                MainUI.ShowMenu = !MainUI.ShowMenu;
        }


        private void DefineHitboxes()
        {
            // All commented hitboxes are not implemented yet

            HitboxDefinition.Define<BoxCollider>(
                nameof(BoxCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#DB220D"),
                    ColorExtensions.HexToColor("#DB220D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<SphereCollider>(
                nameof(SphereCollider),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#0D2FDB"),
                    ColorExtensions.HexToColor("#0D2FDB"),
                    RoundedHitboxAlgorithm.LatitudeLongitude
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CapsuleCollider>(
                nameof(CapsuleCollider),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#28DB0D"),
                    ColorExtensions.HexToColor("#28DB0D"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<MeshCollider>(
                nameof(MeshCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#DBDB0D"),
                    ColorExtensions.HexToColor("#DBDB0D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<WheelCollider>(
                nameof(WheelCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#DB7B0D"),
                    ColorExtensions.HexToColor("#DB7B0D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<TerrainCollider>(
                nameof(TerrainCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#A020F0"),
                    ColorExtensions.HexToColor("#A020F0")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<BoxCollider2D>(
                nameof(BoxCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#FF19AF"),
                    ColorExtensions.HexToColor("#FF19AF")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CircleCollider2D>(
                nameof(CircleCollider2D),
                new RoundedHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#039AFF"),
                    ColorExtensions.HexToColor("#039AFF")
                ),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<CapsuleCollider2D>(
                nameof(CapsuleCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#633310"),
                    ColorExtensions.HexToColor("#633310")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<CharacterController>(
                nameof(CharacterController),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#8A2BE2"),
                    ColorExtensions.HexToColor("#8A2BE2"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<PolygonCollider2D>(
                nameof(PolygonCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#000000"),
                    ColorExtensions.HexToColor("#000000")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<EdgeCollider2D>(
                nameof(EdgeCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#FFFFFF"),
                    ColorExtensions.HexToColor("#FFFFFF")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CompositeCollider2D>(
                nameof(CompositeCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#363636"),
                    ColorExtensions.HexToColor("#363636")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<NavMeshObstacle>(
                nameof(NavMeshObstacle),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    ColorExtensions.HexToColor("#008080"),
                    ColorExtensions.HexToColor("#008080"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.BoxNavMeshObstacle | HitboxesFlags.CapsuleNavMeshObstacle
            );
        }
    }
}