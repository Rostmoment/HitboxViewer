using HitboxViewer.Configs;
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
        public HitboxDefinition hitboxType;

        public virtual GameObject BuildCategory(GameObject editorContent)
        {
            content = UIFactory.CreateVerticalGroup(editorContent, $"HitboxConfig{hitboxType.Name}", true, false, true, true, 4, default, new Color(0.05f, 0.05f, 0.05f));
            content.SetActive(false);

            GameObject bg = UIFactory.CreateHorizontalGroup(content, "TitleBG", true, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            Text title = UIFactory.CreateLabel(bg, $"Title{hitboxType.Name}", hitboxType.Name, TextAnchor.MiddleCenter, default, true, 17);
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 30, minWidth: 200, flexibleWidth: 9999);

            GameObject buttons = UIFactory.CreateHorizontalGroup(content, "Buttons", true, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef enableAll = UIFactory.CreateButton(buttons, "EnableAllFlags", "Enable all flags", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(enableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            enableAll.OnClick += hitboxType.Flags.EnableAll;

            ButtonRef disableAll = UIFactory.CreateButton(buttons, "DisableAllFlags", "Disable all flags", new Color(0.39f, 0, 0f));
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

                GameObject bg = UIFactory.CreateVerticalGroup(content, "BG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

                GameObject toggleObject = UIFactory.CreateToggle(bg, $"Toggle{flag}", out Toggle toggle, out _, new Color(0.1f, 0.1f, 0.1f));
                toggle.isOn = false;
                UIFactory.SetLayoutElement(toggleObject, 1, 25);

                Text description = UIFactory.CreateLabel(bg, $"Description{flag}", flag.GetDescription());
                UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

                ButtonRef apply = UIFactory.CreateButton(bg, $"Apply{flag}", "Apply", new Color(0, 0.39f, 0f));
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
            GameObject bg = UIFactory.CreateHorizontalGroup(content, "ConfigBG", true, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            Text title = UIFactory.CreateLabel(bg, $"Config{hitboxType.Name}", $"Configs for {hitboxType.Name}", TextAnchor.MiddleCenter, default, true, 17);
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 30, minWidth: 200, flexibleWidth: 9999);
            #endregion

            #region start line width
            GameObject startLineWidthBg = UIFactory.CreateVerticalGroup(content, "StartWidthBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            InputFieldRef startLineWidthInput = UIFactory.CreateInputField(startLineWidthBg, "StartLineWidth", "Start width of line");
            startLineWidthInput.Text = config.StartWidth.ToString();
            UIFactory.SetLayoutElement(startLineWidthInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text startLineWidthDescription = UIFactory.CreateLabel(startLineWidthBg, $"DescriptionStartInput", $"Defines the starting width of the LineRenderer used for this hitbox\nDefault: {config.DefaultStartWidth}");
            UIFactory.SetLayoutElement(startLineWidthDescription.gameObject, flexibleWidth: 1);

            GameObject startLineWidthButtonsBg = UIFactory.CreateHorizontalGroup(startLineWidthBg, "StartWidthButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            ButtonRef applyStartLineWidth = UIFactory.CreateButton(startLineWidthButtonsBg, $"ApplyStartLineWidth", "Apply", new Color(0, 0.39f, 0f));
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

            ButtonRef resetStartLineWidth = UIFactory.CreateButton(startLineWidthButtonsBg, $"ResetStartLineWidth", "Reset", new Color(0.39f, 0f, 0f));
            resetStartLineWidth.OnClick += () =>
            {
                config.StartWidth = config.DefaultStartWidth;
                startLineWidthInput.Text = config.DefaultStartWidth.ToString();
            };
            UIFactory.SetLayoutElement(resetStartLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region end line width
            GameObject endLineWidthBg = UIFactory.CreateVerticalGroup(content, "EndWidthBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            InputFieldRef endLineWidthInput = UIFactory.CreateInputField(endLineWidthBg, "EndLineWidth", "End width of line");
            endLineWidthInput.Text = config.EndWidth.ToString();
            UIFactory.SetLayoutElement(endLineWidthInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text endLineWidthDescription = UIFactory.CreateLabel(endLineWidthBg, $"DescriptionEndInput", $"Defines the ending width of the LineRenderer used for this hitbox\nDefault: {config.DefaultEndWidth}");
            UIFactory.SetLayoutElement(endLineWidthDescription.gameObject, flexibleWidth: 1);

            GameObject endLineWidthButtonsBg = UIFactory.CreateHorizontalGroup(endLineWidthBg, "EndWidthButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            ButtonRef applyEndLineWidth = UIFactory.CreateButton(endLineWidthButtonsBg, $"ApplyEndLineWidth", "Apply", new Color(0, 0.39f, 0f));
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

            ButtonRef resetEndLineWidth = UIFactory.CreateButton(endLineWidthButtonsBg, $"ResetEndLineWidth", "Reset", new Color(0.39f, 0f, 0f));
            resetEndLineWidth.OnClick += () =>
            {
                config.EndWidth = config.DefaultEndWidth;
                endLineWidthInput.Text = config.DefaultEndWidth.ToString();
            };
            UIFactory.SetLayoutElement(resetEndLineWidth.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region start line color
            GameObject startColorBg = UIFactory.CreateVerticalGroup(content, "StartColorBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            GameObject imageStartColorBg = UIFactory.CreateHorizontalGroup(startColorBg, "StartColorImageBG", false, false, false, false, 0, default, new Color(0.07f, 0.07f, 0.07f));

            // I don't know why, but simple startColorBg doesn't work, but imageStartColorBg does
            Image startColorImage = UIFactory.CreateUIObject("StartColorImage", imageStartColorBg, new Vector2(100, 25)).AddComponent<Image>();
            UIFactory.SetLayoutElement(startColorImage.gameObject, flexibleWidth: 1);

            InputFieldRef startColorInput = UIFactory.CreateInputField(startColorBg, "StartColorInput", config.StartColor.ToRGBHex());
            UIFactory.SetLayoutElement(startColorInput.Component.gameObject, flexibleWidth: 1);

            Text startColorDescription = UIFactory.CreateLabel(startColorBg, "DescriptionStartColor", "Start color of the hitbox outline");
            UIFactory.SetLayoutElement(startColorDescription.gameObject, flexibleWidth: 1);

            startColorImage.color = config.StartColor;

            GameObject startColorButtonsBg = UIFactory.CreateHorizontalGroup(content, "StartColorButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            ButtonRef applyStartColor = UIFactory.CreateButton(startColorButtonsBg, $"StartColorApply", "Apply", new Color(0f, 0.39f, 0f));
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

            ButtonRef resetStartColor = UIFactory.CreateButton(startColorButtonsBg, $"StartColorReset", "Reset", new Color(0.39f, 0f, 0f));
            UIFactory.SetLayoutElement(resetStartColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            resetStartColor.OnClick += () =>
            {
                startColorImage.color = config.DefaultStartColor;
                config.StartColor = config.DefaultStartColor;
                startColorInput.Text = config.DefaultStartColor.ToRGBHex();
            };
            #endregion

            #region end line color
            GameObject endColorBg = UIFactory.CreateVerticalGroup(content, "EndColorBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            GameObject imageEndColorBg = UIFactory.CreateHorizontalGroup(endColorBg, "EndColorImageBG", false, false, false, false, 0, default, new Color(0.07f, 0.07f, 0.07f));

            Image endColorImage = UIFactory.CreateUIObject("EndColorImage", imageEndColorBg, new Vector2(100, 25)).AddComponent<Image>();
            UIFactory.SetLayoutElement(endColorImage.gameObject, flexibleWidth: 1);

            InputFieldRef endColorInput = UIFactory.CreateInputField(endColorBg, "EndColorInput", config.EndColor.ToRGBHex());
            UIFactory.SetLayoutElement(endColorInput.Component.gameObject, flexibleWidth: 1);

            Text endColorDescription = UIFactory.CreateLabel(endColorBg, "DescriptionEndColor", "End color of the hitbox outline");
            UIFactory.SetLayoutElement(endColorDescription.gameObject, flexibleWidth: 1);

            endColorImage.color = config.EndColor;

            GameObject endColorButtonsBg = UIFactory.CreateHorizontalGroup(content, "EndColorButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyEndColor = UIFactory.CreateButton(endColorButtonsBg, "EndColorApply", "Apply", new Color(0f, 0.39f, 0f));
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

            ButtonRef resetEndColor = UIFactory.CreateButton(endColorButtonsBg, "EndColorReset", "Reset", new Color(0.39f, 0f, 0f));
            UIFactory.SetLayoutElement(resetEndColor.Component.gameObject, 100, 25, 100, 25, 100, 25);
            resetEndColor.OnClick += () =>
            {
                endColorImage.color = config.DefaultEndColor;
                config.EndColor = config.DefaultEndColor;
                endColorInput.Text = config.DefaultEndColor.ToRGBHex();
            };
            #endregion
        }
    }
}
