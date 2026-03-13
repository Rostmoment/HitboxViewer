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
                    Color.HexToColor("#DB220D"),
                    Color.HexToColor("#DB220D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<SphereCollider>(
                nameof(SphereCollider),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    Color.HexToColor("#0D2FDB"),
                    Color.HexToColor("#0D2FDB"),
                    RoundedHitboxAlgorithm.LatitudeLongitude
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CapsuleCollider>(
                nameof(CapsuleCollider),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    Color.HexToColor("#28DB0D"),
                    Color.HexToColor("#28DB0D"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );


            HitboxDefinition.Define<CharacterController>(
                nameof(CharacterController),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    Color.HexToColor("#8A2BE2"),
                    Color.HexToColor("#8A2BE2"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<MeshCollider>(
                nameof(MeshCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#DBDB0D"),
                    Color.HexToColor("#DBDB0D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<WheelCollider>(
                nameof(WheelCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#DB7B0D"),
                    Color.HexToColor("#DB7B0D")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<TerrainCollider>(
                nameof(TerrainCollider),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#A020F0"),
                    Color.HexToColor("#A020F0")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<BoxCollider2D>(
                nameof(BoxCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#FF19AF"),
                    Color.HexToColor("#FF19AF")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CircleCollider2D>(
                nameof(CircleCollider2D),
                new RoundedHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#039AFF"),
                    Color.HexToColor("#039AFF")
                ),
                new RoundedHitboxUI(),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<CapsuleCollider2D>(
                nameof(CapsuleCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#633310"),
                    Color.HexToColor("#633310")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<PolygonCollider2D>(
                nameof(PolygonCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#000000"),
                    Color.HexToColor("#000000")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            /*
            HitboxDefinition.Define<EdgeCollider2D>(
                nameof(EdgeCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#FFFFFF"),
                    Color.HexToColor("#FFFFFF")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );

            HitboxDefinition.Define<CompositeCollider2D>(
                nameof(CompositeCollider2D),
                new BaseHitboxConfig(
                    KeyCode.None,
                    Color.HexToColor("#363636"),
                    Color.HexToColor("#363636")
                ),
                HitboxesFlags.Trigger | HitboxesFlags.NotTrigger
            );
            */

            HitboxDefinition.Define<NavMeshObstacle>(
                nameof(NavMeshObstacle),
                new RoundedHitboxConfig3D(
                    KeyCode.None,
                    Color.HexToColor("#008080"),
                    Color.HexToColor("#008080"),
                    RoundedHitboxAlgorithm.ThreeAxis
                ),
                new RoundedHitbox3DUI(),
                HitboxesFlags.BoxNavMeshObstacle | HitboxesFlags.CapsuleNavMeshObstacle
            );
        }
    }
}