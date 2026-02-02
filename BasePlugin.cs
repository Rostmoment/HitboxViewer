using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HitboxViewer.Displayers;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        private static Dictionary<Type, Type> types = new Dictionary<Type, Type>()
        {
            [typeof(BoxCollider)] = typeof(BoxColliderDisplayer),
        };

        public static Harmony HarmonyInstance { private set; get; }
        public static BasePlugin Instance { private set; get; }
        public static new ManualLogSource Logger { private set; get; }

        private int updateCounter;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;
            HarmonyInstance = new Harmony(PluginInfo.GUID);
            HitboxViewerConfig.Initialize();
        }

        private void Update()
        {
            if (updateCounter < 0)
                return;

            if (updateCounter > 0)
            {
                updateCounter--;
                return;
            }

            updateCounter = HitboxViewerConfig.UpdateRate;

            foreach (Collider collider in GameObject.FindObjectsOfType<Collider>())
            {
                if (types.TryGetValue(collider.GetType(), out Type displayerType))
                {
                    BaseDisplayer displayer = BaseDisplayer.GetOrAdd(collider, displayerType);
                    displayer.Visualize();
                }
            }
        }
    }
}