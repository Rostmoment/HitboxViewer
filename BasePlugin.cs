using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HitboxViewer.Configs;
using HitboxViewer.Displayers;
using HitboxViewer.Displayers.Colliders;
using HitboxViewer.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HitboxViewer
{
    /*
     * git add --all
     * git commit -m "Message"
     * git pull origin master --rebase
     * git push origin master
     */
    class PluginInfo
    {
        public const string GUID = "rost.moment.unity.hitboxviewer";
        public const string VERSION = "0.1.0";
        public const string NAME = "Hitbox Viewer";
    }
   

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {

        private static Harmony HarmonyInstance { set; get; }
        public static BasePlugin Instance { private set; get; }
        public static new ManualLogSource Logger { private set; get; }
        public static GameObject MainObject { private set; get; }


        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;
            HarmonyInstance = new Harmony(PluginInfo.GUID);
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
    }
}