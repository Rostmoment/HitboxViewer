using HitboxViewer.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
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

        public override string Name => $"{PluginInfo.NAME} V{PluginInfo.VERSION}";
        public override int MinWidth => 750;
        public override int MinHeight => 750;

        public override Vector2 DefaultAnchorMin => new Vector2(0.2f, 0.02f);
        public override Vector2 DefaultAnchorMax => Vector2.one;

        public GameObject RootObject => Owner?.RootObject;

        private GameObject hitboxesButtons;
        private GameObject editorContent;
        private GameObject currentCategory;

        private static CanvasGroup canvasGroup;
        public static float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        #endregion

        #region overrides
        protected override void ConstructPanelContent()
        {
        }
        protected override void OnClosePanelClicked()
        {
            base.OnClosePanelClicked();

            ShowMenu = false;
        }


        #endregion

        #region static methods
        public static void InitializeUI()
        {
            UIBase uiBase = UniversalUI.RegisterUI(PluginInfo.GUID, null);
            canvasGroup = uiBase.Canvas.gameObject.AddComponent<CanvasGroup>();
            Alpha = HitboxViewerConfig.MenuAlpha;
            CreateMainUI(uiBase);

            Instance.AddButtonsUnderPanel();

            Instance.CreateScrollView();
            for (int i = 0; i < HitboxDefinition.all.Count; i++)
                Instance.AddButton(HitboxDefinition.all[i]);

            if (HitboxViewerConfig.HideOnStart)
                ShowMenu = false;
        }

        private static void CreateMainUI(UIBase uIBase)
        {
            if (!Instance.IsNullOrDestroyed())
                return;

            new MainUI(uIBase);
        }
        #endregion

        private void CreateScrollView()
        {
            GameObject horiGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "Main", true, true, true, true, 2, default, new Color(0.08f, 0.08f, 0.08f));
            GameObject ctgList = UIFactory.CreateScrollView(horiGroup, "CategoryList", out hitboxesButtons, out _, new Color(0.1f, 0.1f, 0.1f));

            UIFactory.SetLayoutElement(ctgList, minWidth: 300, flexibleWidth: 0);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(hitboxesButtons, spacing: 3);

            GameObject editor = UIFactory.CreateScrollView(horiGroup, "HitboxEditor", out editorContent, out _, new Color(0.05f, 0.05f, 0.05f));
            UIFactory.SetLayoutElement(editor, flexibleWidth: 9999);
        }

        private ButtonRef AddButton(HitboxDefinition type)
        {
            ButtonRef btn = UIFactory.CreateButton(hitboxesButtons, $"Button{type.Name}", type.Name);
            UIFactory.SetLayoutElement(btn.Component.gameObject, flexibleWidth: 9999, minHeight: 40, flexibleHeight: 0);
            GameObject myCategory = type.UI.BuildCategory(editorContent);

            btn.OnClick += () =>
            {
                currentCategory?.SetActive(false);
                myCategory.SetActive(true);
                currentCategory = myCategory;
            };
            

            return btn;
        }
        private void AddButtonsUnderPanel()
        {
            #region close
            Button closeButton = TitleBar.GetComponentInChildren<Button>();
            RuntimeHelper.SetColorBlock(closeButton, Color.red, new Color(0.54f, 0.07f, 0.02f), new Color(0.54f, 0.07f, 0.02f));

            Text hideText = closeButton.GetComponentInChildren<Text>();
            hideText.color = Color.white;
            hideText.resizeTextForBestFit = true;
            hideText.resizeTextMinSize = 8;
            hideText.resizeTextMaxSize = 14;
            #endregion

            GameObject titleButtonsGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "TitleBarGroup", true, true, true, true, 2, new Vector4(2, 2, 2, 2));

            ButtonRef hitboxesButton = UIFactory.CreateButton(titleButtonsGroup, "HitboxesButton", "Hitboxes", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(hitboxesButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);
            hitboxesButton.OnClick += () => { hitboxesButtons.SetActive(true); };
            /*
            ButtonRef configButton = UIFactory.CreateButton(titleButtonsGroup, "ConfigButton", "Configs", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(configButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);
            configButton.OnClick += () => { hitboxesButtons.SetActive(false); };

            ButtonRef instancesButton = UIFactory.CreateButton(titleButtonsGroup, "HitboxesInstances", "Instances", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(instancesButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);*/

        }
    }
}
