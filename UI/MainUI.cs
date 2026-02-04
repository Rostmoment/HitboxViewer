using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Config;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using UniverseLib.Utility;

namespace HitboxViewer.UI
{
    public class MainUI : PanelBase
    {
        #region constructor and properties
        public static MainUI Instance;

        public static bool ShowMenu
        {
            get => Instance?.Owner != null && Instance.Owner.Enabled;
            set
            {
                if (Instance?.Owner == null || Instance.RootObject.IsNullOrDestroyed()|| Instance.Owner.Enabled == value)
                    return;

                UniversalUI.SetUIActive(PluginInfo.GUID, value);
                Instance.SetActive(value);
            }
        }

        public MainUI(UIBase owner) : base(owner)
        {
            Instance = this;
        }

        public override string Name => "Hitbox Viewer";
        public override int MinWidth => 750;
        public override int MinHeight => 750;

        public override Vector2 DefaultAnchorMin => new Vector2(0.2f, 0.02f);
        public override Vector2 DefaultAnchorMax => Vector2.one;

        public GameObject RootObject => Owner?.RootObject;
        private static RectTransform NavBarRect { get; set; }

        #endregion


        protected override void ConstructPanelContent()
        {
        }


        public static void InitializeUI()
        {
            UIBase uiBase = UniversalUI.RegisterUI(PluginInfo.GUID, null);
            CreateMainUI(uiBase);
        }

        private void AddButton()
        {
        }
        private static void CreateMainUI(UIBase uIBase)
        {
            if (!Instance.IsNullOrDestroyed())
                return;

            new MainUI(uIBase);
        }

    }
}
