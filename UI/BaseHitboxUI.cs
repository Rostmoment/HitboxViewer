using HitboxViewer.Configs;
using HitboxViewer.Constants;
using HitboxViewer.Extensions;
using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace HitboxViewer.UI
{
    public class BaseHitboxUI
    {
        protected GameObject content;
        protected GameObject configBg;
        public HitboxDefinition hitboxType;

        public virtual GameObject BuildCategory(GameObject editorContent)
        {
            content = UIFactory.CreateVerticalGroup(editorContent, $"HitboxConfig{hitboxType.Name}", true, false, true, true, UIConstants.SPACING, default, UIConstants.mainBackgroundColor);
            content.SetActive(false);

            GameObject bg = UIFactory.CreateHorizontalGroup(content, "TitleBG", true, true, true, true, 0, default, UIConstants.titleBackgroundColor);

            Text title = UIFactory.CreateLabel(bg, $"Title{hitboxType.Name}", hitboxType.Name, TextAnchor.MiddleCenter, default, true, 17);
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 30, minWidth: 200, flexibleWidth: 9999);

            GameObject buttons = UIFactory.CreateHorizontalGroup(content, "Buttons", true, true, true, true, 0, default);

            ButtonRef enableAll = UIFactory.CreateButton(buttons, "EnableAllFlags", "Enable all flags", UIConstants.greenButtonColor);
            UIFactory.SetLayoutElement(enableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            enableAll.OnClick += hitboxType.Flags.EnableAll;

            ButtonRef disableAll = UIFactory.CreateButton(buttons, "DisableAllFlags", "Disable all flags", UIConstants.redButtonColor);
            UIFactory.SetLayoutElement(disableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            disableAll.OnClick += hitboxType.Flags.DisableAll;

            BuildFlagsSettings();
            BuildConfigs();

            return content;
        }

        public virtual void BuildFlagsSettings()
        {
            foreach (HitboxesFlags flag in FlagsExtensions.all)
            {
                if (!hitboxType.Flags.HasFlag(flag))
                    continue;

                GameObject bg = UIFactory.CreateVerticalGroup(content, "BG", false, true, true, true, UIConstants.BLOCK_SPACING, default, UIConstants.flagBackgroundColor);

                Text name = UIFactory.CreateLabel(bg, $"Name{flag}", flag.GetName());
                UIFactory.SetLayoutElement(name.gameObject, flexibleWidth: 1);

                GameObject toggleObject = UIFactory.CreateToggle(bg, $"Toggle{flag}", out Toggle toggle, out Text text);
                text.color = UIConstants.disabledToggleTextColor;
                text.text = "Disabled";
                toggle.isOn = false;
                toggle.onValueChanged.AddListener((val) =>
                {
                    if (val)
                    {
                        text.color = UIConstants.enabledToggleTextColor;
                        text.text = "Enabled";
                    }
                    else
                    {
                        text.color = UIConstants.disabledToggleTextColor;
                        text.text = "Disabled";
                    }
                });
                UIFactory.SetLayoutElement(toggleObject, 1, 25);

                Text description = UIFactory.CreateLabel(bg, $"Description{flag}", flag.GetDescription(), color: UIConstants.flagDescriptionColor);
                UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

                ButtonRef apply = UIFactory.CreateButton(bg, $"Apply{flag}", "Apply", UIConstants.greenButtonColor);
                apply.OnClick += () =>
                {
                    hitboxType.Flags.SetEnabled(toggle.isOn, flag);
                };
                UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);
            }
        }

        public virtual void BuildConfigs()
        {
            BaseHitboxConfig config = hitboxType.Config;

            #region title
            GameObject titleBg = UIFactory.CreateHorizontalGroup(content, "TitgleBG", true, true, true, true, UIConstants.CONFIG_SPACING, default, UIConstants.titleBackgroundColor);

            Text title = UIFactory.CreateLabel(titleBg, $"Config{hitboxType.Name}", $"Configs for {hitboxType.Name}", TextAnchor.MiddleCenter, default, true, 17);
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 30, minWidth: 200, flexibleWidth: 9999);
            #endregion

            configBg = UIFactory.CreateVerticalGroup(content, "ConfigBG", true, true, true, true, UIConstants.CONFIG_SPACING, default, UIConstants.mainBackgroundColor);

            #region start line width
            GameObject startLineWidthBg = CreateConfigBlockBG("StartWidthBG");

            Text startWidthTitle = UIFactory.CreateLabel(startLineWidthBg, $"StartWidthTitle{hitboxType.Name}", $"Start Line Width");
            UIFactory.SetLayoutElement(startWidthTitle.gameObject, minHeight: 25, flexibleWidth: 9999);

            UIFactory.SetLayoutElement(startWidthTitle.gameObject, minHeight: 25, flexibleWidth: 9999);

            InputFieldRef startLineWidthInput = UIFactory.CreateInputField(startLineWidthBg, "StartLineWidth", "Start width of line");
            startLineWidthInput.Text = config.StartWidth.ToString();
            UIFactory.SetLayoutElement(startLineWidthInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text startLineWidthDescription = UIFactory.CreateLabel(startLineWidthBg, $"DescriptionStartInput", $"Defines the starting width of the LineRenderer used for this hitbox\nDefault: {config.DefaultStartWidth}", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(startLineWidthDescription.gameObject, flexibleWidth: 1);

            GameObject startLineWidthButtonsBg = UIFactory.CreateHorizontalGroup(startLineWidthBg, "StartWidthButtonsBG", false, true, true, true, 0, default);
            ButtonRef applyStartLineWidth = UIFactory.CreateButton(startLineWidthButtonsBg, $"ApplyStartLineWidth", "Apply", UIConstants.greenButtonColor);
            applyStartLineWidth.OnClick += () =>
            {
                if (!float.TryParse(startLineWidthInput.Text, out float value))
                {
                    config.StartWidth = config.DefaultStartWidth;
                    startLineWidthInput.Text = config.DefaultStartWidth.ToString();
                }
                config.StartWidth = value;
            };
            UIFactory.SetLayoutElement(applyStartLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetStartLineWidth = UIFactory.CreateButton(startLineWidthButtonsBg, $"ResetStartLineWidth", "Reset", UIConstants.redButtonColor);
            resetStartLineWidth.OnClick += () =>
            {
                config.StartWidth = config.DefaultStartWidth;
                startLineWidthInput.Text = config.DefaultStartWidth.ToString();
            };
            UIFactory.SetLayoutElement(resetStartLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region end line width
            GameObject endLineWidthBg = CreateConfigBlockBG("EndWidthBG");

            Text endWidthTitle = UIFactory.CreateLabel(endLineWidthBg, $"EndWidthTitle{hitboxType.Name}", $"End Line Width");
            UIFactory.SetLayoutElement(endWidthTitle.gameObject, minHeight: 25, flexibleWidth: 9999);

            InputFieldRef endLineWidthInput = UIFactory.CreateInputField(endLineWidthBg, "EndLineWidth", "End width of line");
            endLineWidthInput.Text = config.EndWidth.ToString();
            UIFactory.SetLayoutElement(endLineWidthInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text endLineWidthDescription = UIFactory.CreateLabel(endLineWidthBg, $"DescriptionEndInput", $"Defines the ending width of the LineRenderer used for this hitbox\nDefault: {config.DefaultEndWidth}", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(endLineWidthDescription.gameObject, flexibleWidth: 1);

            GameObject endLineWidthButtonsBg = UIFactory.CreateHorizontalGroup(endLineWidthBg, "EndWidthButtonsBG", false, true, true, true, 0, default);
            ButtonRef applyEndLineWidth = UIFactory.CreateButton(endLineWidthButtonsBg, $"ApplyEndLineWidth", "Apply", UIConstants.greenButtonColor);
            applyEndLineWidth.OnClick += () =>
            {
                if (!float.TryParse(endLineWidthInput.Text, out float value))
                {
                    config.EndWidth = config.DefaultEndWidth;
                    endLineWidthInput.Text = config.DefaultEndWidth.ToString();
                }
                config.EndWidth = value;
            };
            UIFactory.SetLayoutElement(applyEndLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetEndLineWidth = UIFactory.CreateButton(endLineWidthButtonsBg, $"ResetEndLineWidth", "Reset", UIConstants.redButtonColor);
            resetEndLineWidth.OnClick += () =>
            {
                config.EndWidth = config.DefaultEndWidth;
                endLineWidthInput.Text = config.DefaultEndWidth.ToString();
            };
            UIFactory.SetLayoutElement(resetEndLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region start line color
            GameObject startColorBg = CreateConfigBlockBG("StartColorBG");

            Text startColorTitle = UIFactory.CreateLabel(startColorBg, $"StartColorTitle{hitboxType.Name}", $"Start Line Color");
            UIFactory.SetLayoutElement(startColorTitle.gameObject, minHeight: 25, flexibleWidth: 9999);

            GameObject imageStartColorBg = UIFactory.CreateHorizontalGroup(startColorBg, "StartColorImageBG", false, false, false, false, 0, default, UIConstants.configBackgroundColor);
            // I don't know why, but simple startColorBg doesn't work, but imageStartColorBg does
            Image startColorImage = UIFactory.CreateUIObject("StartColorImage", imageStartColorBg, new Vector2(100, 25)).AddComponent<Image>();
            UIFactory.SetLayoutElement(startColorImage.gameObject, flexibleWidth: 1);

            InputFieldRef startColorInput = UIFactory.CreateInputField(startColorBg, "StartColorInput", config.StartColor.ToRGBHex());
            startColorInput.Text = config.StartColor.ToRGBHex();
            UIFactory.SetLayoutElement(startColorInput.Component.gameObject, flexibleWidth: 1);

            Text startColorDescription = UIFactory.CreateLabel(startColorBg, "DescriptionStartColor", "Start color in hex format of the hitbox outline", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(startColorDescription.gameObject, flexibleWidth: 1);

            startColorImage.color = config.StartColor;

            GameObject startColorButtonsBg = UIFactory.CreateHorizontalGroup(startColorBg, "StartColorButtonsBG", false, true, true, true, 0, default);
            ButtonRef applyStartColor = UIFactory.CreateButton(startColorButtonsBg, $"StartColorApply", "Apply", UIConstants.greenButtonColor);
            UIFactory.SetLayoutElement(applyStartColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            applyStartColor.OnClick += () =>
            {
                string hex = startColorInput.Text;

                if (!hex.StartsWith("#"))
                    hex = "#" + hex;

                if (ColorUtility.TryParseHtmlString(hex, out Color color))
                {
                    startColorImage.color = color;
                    config.StartColor = color;
                }
                else
                {
                    startColorImage.color = config.DefaultStartColor;
                    config.StartColor = config.DefaultStartColor;
                    startColorInput.Text = config.DefaultStartColor.ToRGBHex();
                }
            };

            ButtonRef resetStartColor = UIFactory.CreateButton(startColorButtonsBg, $"StartColorReset", "Reset", UIConstants.redButtonColor);
            UIFactory.SetLayoutElement(resetStartColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            resetStartColor.OnClick += () =>
            {
                startColorImage.color = config.DefaultStartColor;
                config.StartColor = config.DefaultStartColor;
                startColorInput.Text = config.DefaultStartColor.ToRGBHex();
            };
            #endregion

            #region end line color
            GameObject endColorBg = CreateConfigBlockBG("EndColorBG");

            Text endColorTitle = UIFactory.CreateLabel(endColorBg, $"EndColorTitle{hitboxType.Name}", $"End Line Color");
            UIFactory.SetLayoutElement(endColorTitle.gameObject, minHeight: 25, flexibleWidth: 9999);

            GameObject imageEndColorBg = UIFactory.CreateHorizontalGroup(endColorBg, "EndColorImageBG", false, false, false, false, 0, default, UIConstants.configBackgroundColor);
            Image endColorImage = UIFactory.CreateUIObject("EndColorImage", imageEndColorBg, new Vector2(100, 25)).AddComponent<Image>();
            UIFactory.SetLayoutElement(endColorImage.gameObject, flexibleWidth: 1);

            InputFieldRef endColorInput = UIFactory.CreateInputField(endColorBg, "EndColorInput", config.EndColor.ToRGBHex());
            endColorInput.Text = config.EndColor.ToRGBHex();
            UIFactory.SetLayoutElement(endColorInput.Component.gameObject, flexibleWidth: 1);

            Text endColorDescription = UIFactory.CreateLabel(endColorBg, "DescriptionEndColor", "End color in hex format of the hitbox outline", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(endColorDescription.gameObject, flexibleWidth: 1);

            endColorImage.color = config.EndColor;

            GameObject endColorButtonsBg = UIFactory.CreateHorizontalGroup(endColorBg, "EndColorButtonsBG", false, true, true, true, 0, default);

            ButtonRef applyEndColor = UIFactory.CreateButton(endColorButtonsBg, "EndColorApply", "Apply", UIConstants.greenButtonColor);
            UIFactory.SetLayoutElement(applyEndColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            applyEndColor.OnClick += () =>
            {
                string hex = startColorInput.Text;

                if (!hex.StartsWith("#"))
                    hex = "#" + hex;

                if (ColorUtility.TryParseHtmlString(hex, out Color color))
                {
                    endColorImage.color = color;
                    config.EndColor = color;
                }
                else
                {
                    endColorImage.color = config.DefaultEndColor;
                    config.EndColor = config.DefaultEndColor;
                    endColorInput.Text = config.DefaultEndColor.ToRGBHex();
                }
            };

            ButtonRef resetEndColor = UIFactory.CreateButton(endColorButtonsBg, "EndColorReset", "Reset", UIConstants.redButtonColor);
            UIFactory.SetLayoutElement(resetEndColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            resetEndColor.OnClick += () =>
            {
                endColorImage.color = config.DefaultEndColor;
                config.EndColor = config.DefaultEndColor;
                endColorInput.Text = config.DefaultEndColor.ToRGBHex();
            };
            #endregion
        }

        protected GameObject CreateConfigBlockBG(string name)
        {
            if (configBg.IsNullOrDestroyed())
                throw new ArgumentNullException("Config bg is null or destroyed!");

            return UIFactory.CreateVerticalGroup(configBg, name, false, true, true, true, UIConstants.BLOCK_SPACING, default, UIConstants.configBackgroundColor);
        }
    }
}
