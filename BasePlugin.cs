using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using HitboxViewer.Displayers;
using HitboxViewer.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public const string VERSION = "0.0.8";
        public const string NAME = "Hitbox Viewer";
    }
   

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class BasePlugin : BaseUnityPlugin
    {
        public static Vector3 VectorNaN => new Vector3(float.NaN, float.NaN, float.NaN);
        public static CollidersVisualizationMode ColliderVisualize { set; get; }
        public static NavMeshObstacleVisualizationMode NavMeshObstacleVisualize { set; get; }

        public static int FrameCounter { private set; get; }
        public static string HitboxMode { private set; get; }
        public static Harmony HarmonyInstance { private set; get; }
        public static BasePlugin Instance { private set; get; }
        public static new ManualLogSource Logger { private set; get; }

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;
            HarmonyInstance = new Harmony(PluginInfo.GUID);
            HarmonyInstance.TryPatchAll();
            HitboxViewConfig.Initialize();
        }
        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 24;
            HitboxMode = $"Colliders: {ColliderVisualize.ToName()}\n" +
                $"NavMeshObstacle: {NavMeshObstacleVisualize.ToName()}\n";
            GUI.Label(new Rect(10f, 70f, 100f, 50f), HitboxMode, style);
        }


        private void Update()
        {
            ColliderDisplayer.UpdatePre();
            NavMeshObstacleDisplayer.UpdatePre();

            if (HitboxViewConfig.UpdateRate <= 0)
                return;

            FrameCounter -= 1;
            if (FrameCounter <= 0)
            {
                List<Component> hitboxes = new List<Component>();
                if (BasePlugin.ColliderVisualize != CollidersVisualizationMode.Hide)
                {
                    hitboxes.AddRange(GameObject.FindObjectsOfType<Collider>());
                    hitboxes.AddRange(GameObject.FindObjectsOfType<Collider2D>());

                    for (int i = 0; i < hitboxes.Count; i++)
                    {
                        hitboxes[i].gameObject.GetOrAddComponent<ColliderDisplayer>().Visualize();

                    }
                }
                hitboxes.Clear();

                if (BasePlugin.NavMeshObstacleVisualize != NavMeshObstacleVisualizationMode.Hide)
                {
                    hitboxes.AddRange(GameObject.FindObjectsOfType<NavMeshObstacle>());
                    for (int i = 0; i < hitboxes.Count; i++)
                        hitboxes[i].gameObject.GetOrAddComponent<NavMeshObstacleDisplayer>().Visualize();
                }
                hitboxes.Clear();
                FrameCounter = HitboxViewConfig.UpdateRate;
            }
        }
    }
}