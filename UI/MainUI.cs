using HitboxViewer.Configs;
using HitboxViewer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (Instance?.Owner == null || Instance.RootObject.IsNullOrDestroyed() || Instance.Owner.Enabled == value)
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
        public override Vector2 DefaultAnchorMax => new Vector2(0.8f, 0.08f);

        public GameObject RootObject => Owner?.RootObject;

        private GameObject hitboxesButtons;
        private GameObject editorContent;
        private GameObject currentCategory;

        private readonly List<ButtonRef> categoryButtons = new List<ButtonRef>();
        private ButtonRef currentCategoryButton;

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

            foreach (var data in HitboxDefinition.definitions)
                Instance.AddButton(data.Value);

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
            GameObject horiGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "Main", true, true, true, true, 2, default, UIConstants.mainBackgroundColor);
            GameObject ctgList = UIFactory.CreateScrollView(horiGroup, "CategoryList", out hitboxesButtons, out _, UIConstants.sidebarBackgroundColor);

            UIFactory.SetLayoutElement(ctgList, minWidth: 300, flexibleWidth: 0);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(hitboxesButtons, spacing: UIConstants.SPACING);

            GameObject editor = UIFactory.CreateScrollView(horiGroup, "HitboxEditor", out editorContent, out _, UIConstants.mainBackgroundColor);
            UIFactory.SetLayoutElement(editor, flexibleWidth: 9999);
        }

        private ButtonRef AddButton(HitboxDefinition type)
        {
            ButtonRef btn = UIFactory.CreateButton(hitboxesButtons, $"Button{type.Name}", type.Name);
            UIFactory.SetLayoutElement(btn.Component.gameObject, flexibleWidth: 9999, minHeight: 42, flexibleHeight: 0);
            StyleSidebarButton(btn, selected: false);

            Text label = btn.Component.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.alignment = TextAnchor.MiddleLeft;
                label.color = UIConstants.textPrimaryColor;
                label.fontStyle = FontStyle.Bold;
                label.rectTransform.offsetMin = new Vector2(14, label.rectTransform.offsetMin.y);
            }

            GameObject myCategory = type.UI.BuildCategory(editorContent);

            categoryButtons.Add(btn);

            btn.OnClick += () => ActivateCategory(btn, myCategory);

            if (categoryButtons.Count == 1)
                ActivateCategory(btn, myCategory);

            return btn;
        }

        private void ActivateCategory(ButtonRef btn, GameObject category)
        {
            currentCategory?.SetActive(false);
            category.SetActive(true);
            currentCategory = category;

            SelectCategoryButton(btn);
        }

        private void SelectCategoryButton(ButtonRef selected)
        {
            if (currentCategoryButton != null)
                StyleSidebarButton(currentCategoryButton, selected: false);

            StyleSidebarButton(selected, selected: true);
            currentCategoryButton = selected;
        }

        private static void StyleSidebarButton(ButtonRef btn, bool selected)
        {
            if (selected)
                RuntimeHelper.SetColorBlock(btn.Component, UIConstants.accentColor, UIConstants.accentColorHover, UIConstants.accentColorPressed);
            else
                RuntimeHelper.SetColorBlock(btn.Component, UIConstants.sidebarButtonColor, UIConstants.sidebarButtonHoverColor, UIConstants.sidebarButtonColor);
        }

        private void AddButtonsUnderPanel()
        {
            #region close
            Button closeButton = TitleBar.GetComponentInChildren<Button>();
            RuntimeHelper.SetColorBlock(closeButton, UIConstants.closeButtonColor, UIConstants.closeButtonHoverColor, UIConstants.closeButtonPressedColor);

            Text hideText = closeButton.GetComponentInChildren<Text>();
            hideText.color = UIConstants.textPrimaryColor;
            hideText.resizeTextForBestFit = true;
            hideText.resizeTextMinSize = 8;
            hideText.resizeTextMaxSize = 14;
            #endregion

            GameObject titleButtonsGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "TitleBarGroup", true, true, true, true, 2, new Vector4(2, 2, 2, 2), UIConstants.titleBackgroundColor);

            ButtonRef hitboxesButton = UIFactory.CreateButton(titleButtonsGroup, "HitboxesButton", "Hitboxes", UIConstants.accentColor);
            UIFactory.SetLayoutElement(hitboxesButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);
            RuntimeHelper.SetColorBlock(hitboxesButton.Component, UIConstants.accentColor, UIConstants.accentColorHover, UIConstants.accentColorPressed);
            hitboxesButton.OnClick += () => { hitboxesButtons.SetActive(true); };

            Text navLabel = hitboxesButton.Component.GetComponentInChildren<Text>();
            if (navLabel != null)
            {
                navLabel.color = UIConstants.textPrimaryColor;
                navLabel.fontStyle = FontStyle.Bold;
            }

            /*
            ButtonRef configButton = UIFactory.CreateButton(titleButtonsGroup, "ConfigButton", "Configs", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(configButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);
            configButton.OnClick += () => { hitboxesButtons.SetActive(false); };

            ButtonRef instancesButton = UIFactory.CreateButton(titleButtonsGroup, "HitboxesInstances", "Instances", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(instancesButton.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 999);*/

        }
    }
}