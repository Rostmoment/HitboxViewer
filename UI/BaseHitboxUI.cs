using HitboxViewer.Configs;
using HitboxViewer.Constants;
using HitboxViewer.Extensions;
using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using UniverseLib;
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

            Text title = UIFactory.CreateLabel(bg, $"Title{hitboxType.Name}", hitboxType.Name, TextAnchor.MiddleCenter, default, true, 18);
            title.color = UIConstants.textPrimaryColor;
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 36, minWidth: 200, flexibleWidth: 9999);

            GameObject buttons = UIFactory.CreateHorizontalGroup(content, "Buttons", true, true, true, true, UIConstants.SPACING, default);

            ButtonRef enableAll = UIFactory.CreateButton(buttons, "EnableAllFlags", "Enable all flags", UIConstants.greenButtonColor);
            UIFactory.SetLayoutElement(enableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            RuntimeHelper.SetColorBlock(enableAll.Component, UIConstants.greenButtonColor, UIConstants.greenButtonHoverColor, UIConstants.greenButtonPressedColor);
            enableAll.OnClick += hitboxType.Flags.EnableAll;

            ButtonRef disableAll = UIFactory.CreateButton(buttons, "DisableAllFlags", "Disable all flags", UIConstants.redButtonColor);
            UIFactory.SetLayoutElement(disableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            RuntimeHelper.SetColorBlock(disableAll.Component, UIConstants.redButtonColor, UIConstants.redButtonHoverColor, UIConstants.redButtonPressedColor);
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

                GameObject bg = UIFactory.CreateVerticalGroup(content, $"Flag{flag}BG", false, true, true, true, UIConstants.BLOCK_SPACING, default, UIConstants.flagBackgroundColor);

                Text name = UIFactory.CreateLabel(bg, $"Name{flag}", flag.Name, TextAnchor.MiddleLeft, UIConstants.textPrimaryColor);
                name.fontStyle = FontStyle.Bold;
                UIFactory.SetLayoutElement(name.gameObject, flexibleWidth: 1, minHeight: 22);

                GameObject toggleObject = UIFactory.CreateToggle(bg, $"Toggle{flag}", out Toggle toggle, out Text text);
                text.color = UIConstants.disabledToggleTextColor;
                text.text = "Disabled";
                text.fontStyle = FontStyle.Bold;
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

                Text description = UIFactory.CreateLabel(bg, $"Description{flag}", flag.Description, color: UIConstants.flagDescriptionColor);
                UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

                GameObject flagButtonsBg = UIFactory.CreateHorizontalGroup(bg, $"Flag{flag}ButtonsBG", false, true, true, true, UIConstants.SPACING, default);

                ButtonRef apply = UIFactory.CreateButton(flagButtonsBg, $"Apply{flag}", "Apply", UIConstants.greenButtonColor);
                RuntimeHelper.SetColorBlock(apply.Component, UIConstants.greenButtonColor, UIConstants.greenButtonHoverColor, UIConstants.greenButtonPressedColor);
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
            GameObject titleBg = UIFactory.CreateHorizontalGroup(content, "ConfigsTitleBG", true, true, true, true, UIConstants.CONFIG_SPACING, default, UIConstants.titleBackgroundColor);

            Text title = UIFactory.CreateLabel(titleBg, $"Config{hitboxType.Name}", $"Configs for {hitboxType.Name}", TextAnchor.MiddleCenter, default, true, 17);
            title.color = UIConstants.textPrimaryColor;
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 32, minWidth: 200, flexibleWidth: 9999);
            #endregion

            configBg = UIFactory.CreateVerticalGroup(content, "ConfigBG", true, true, true, true, UIConstants.CONFIG_SPACING, default, UIConstants.mainBackgroundColor);

            CreateFloatSetting(
                "StartLineWidth",
                "Start Line Width",
                "Defines the starting width of the LineRenderer used for this hitbox",
                config.StartWidth,
                config.DefaultStartWidth,
                value => config.StartWidth = value);

            CreateFloatSetting(
                "EndLineWidth",
                "End Line Width",
                "Defines the ending width of the LineRenderer used for this hitbox",
                config.EndWidth,
                config.DefaultEndWidth,
                value => config.EndWidth = value);

            CreateColorSetting(
                "StartColor",
                "Start Line Color",
                "Start color in hex format of the hitbox outline",
                config.StartColor,
                config.DefaultStartColor,
                color => config.StartColor = color);

            CreateColorSetting(
                "EndColor",
                "End Line Color",
                "End color in hex format of the hitbox outline",
                config.EndColor,
                config.DefaultEndColor,
                color => config.EndColor = color);
        }

        protected GameObject CreateConfigBlockBG(string name)
        {
            if (configBg.IsNullOrDestroyed())
                throw new ArgumentNullException("Config bg is null or destroyed!");

            return UIFactory.CreateVerticalGroup(configBg, name, false, true, true, true, UIConstants.BLOCK_SPACING, default, UIConstants.configBackgroundColor);
        }

        protected InputFieldRef CreateFloatSetting(string idPrefix, string title, string description, float currentValue, float defaultValue, Action<float> onApply)
        {
            GameObject bg = CreateConfigBlockBG($"{idPrefix}BG");

            Text titleLabel = UIFactory.CreateLabel(bg, $"{idPrefix}Title", title, TextAnchor.MiddleLeft, UIConstants.textPrimaryColor);
            titleLabel.fontStyle = FontStyle.Bold;
            UIFactory.SetLayoutElement(titleLabel.gameObject, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            InputFieldRef input = UIFactory.CreateInputField(bg, $"{idPrefix}Input", title);
            input.Text = currentValue.ToString();
            UIFactory.SetLayoutElement(input.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text desc = UIFactory.CreateLabel(bg, $"{idPrefix}Description", $"{description}\nDefault: {defaultValue}", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(desc.gameObject, flexibleWidth: 1);

            GameObject buttonsBg = UIFactory.CreateHorizontalGroup(bg, $"{idPrefix}ButtonsBG", false, true, true, true, UIConstants.SPACING, default);

            ButtonRef apply = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Apply", "Apply", UIConstants.greenButtonColor);
            RuntimeHelper.SetColorBlock(apply.Component, UIConstants.greenButtonColor, UIConstants.greenButtonHoverColor, UIConstants.greenButtonPressedColor);
            apply.OnClick += () =>
            {
                if (!float.TryParse(input.Text, out float value))
                {
                    value = defaultValue;
                    input.Text = defaultValue.ToString();
                }
                onApply(value);
            };
            UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef reset = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Reset", "Reset", UIConstants.redButtonColor);
            RuntimeHelper.SetColorBlock(reset.Component, UIConstants.redButtonColor, UIConstants.redButtonHoverColor, UIConstants.redButtonPressedColor);
            reset.OnClick += () =>
            {
                input.Text = defaultValue.ToString();
                onApply(defaultValue);
            };
            UIFactory.SetLayoutElement(reset.Component.gameObject, 100, 25, 100, 25, 100, 25);

            return input;
        }

        protected InputFieldRef CreateColorSetting(string idPrefix, string title, string description, Color currentValue, Color defaultValue, Action<Color> onApply)
        {
            GameObject bg = CreateConfigBlockBG($"{idPrefix}BG");

            Text titleLabel = UIFactory.CreateLabel(bg, $"{idPrefix}Title", title, TextAnchor.MiddleLeft, UIConstants.textPrimaryColor);
            titleLabel.fontStyle = FontStyle.Bold;
            UIFactory.SetLayoutElement(titleLabel.gameObject, minHeight: 25, flexibleWidth: 9999);

            GameObject imageBg = UIFactory.CreateHorizontalGroup(bg, $"{idPrefix}ImageBG", false, false, false, false, 0, default, UIConstants.configBackgroundColor);
            Image preview = UIFactory.CreateUIObject($"{idPrefix}Image", imageBg, new Vector2(100, 25)).AddComponent<Image>();
            UIFactory.SetLayoutElement(preview.gameObject, flexibleWidth: 1);
            preview.color = currentValue;

            InputFieldRef input = UIFactory.CreateInputField(bg, $"{idPrefix}Input", currentValue.ToRGBHex());
            input.Text = currentValue.ToRGBHex();
            UIFactory.SetLayoutElement(input.Component.gameObject, flexibleWidth: 1);

            Text desc = UIFactory.CreateLabel(bg, $"{idPrefix}Description", description, color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(desc.gameObject, flexibleWidth: 1);

            GameObject buttonsBg = UIFactory.CreateHorizontalGroup(bg, $"{idPrefix}ButtonsBG", false, true, true, true, UIConstants.SPACING, default);

            ButtonRef apply = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Apply", "Apply", UIConstants.greenButtonColor);
            RuntimeHelper.SetColorBlock(apply.Component, UIConstants.greenButtonColor, UIConstants.greenButtonHoverColor, UIConstants.greenButtonPressedColor);
            UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);
            apply.OnClick += () =>
            {
                string hex = input.Text;

                if (!hex.StartsWith("#"))
                    hex = "#" + hex;

                if (ColorUtility.TryParseHtmlString(hex, out Color color))
                {
                    preview.color = color;
                    onApply(color);
                }
                else
                {
                    preview.color = defaultValue;
                    onApply(defaultValue);
                    input.Text = defaultValue.ToRGBHex();
                }
            };

            ButtonRef reset = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Reset", "Reset", UIConstants.redButtonColor);
            RuntimeHelper.SetColorBlock(reset.Component, UIConstants.redButtonColor, UIConstants.redButtonHoverColor, UIConstants.redButtonPressedColor);
            UIFactory.SetLayoutElement(reset.Component.gameObject, 100, 25, 100, 25, 100, 25);
            reset.OnClick += () =>
            {
                preview.color = defaultValue;
                onApply(defaultValue);
                input.Text = defaultValue.ToRGBHex();
            };

            return input;
        }

        protected Dropdown CreateDropdownSetting(string idPrefix, string title, string description, IEnumerable<string> options, int currentIndex, int defaultIndex, Action<int> onApply)
        {
            GameObject bg = CreateConfigBlockBG($"{idPrefix}BG");

            Text titleLabel = UIFactory.CreateLabel(bg, $"{idPrefix}Title", title, TextAnchor.MiddleLeft, UIConstants.textPrimaryColor);
            titleLabel.fontStyle = FontStyle.Bold;
            UIFactory.SetLayoutElement(titleLabel.gameObject, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            GameObject dropObj = UIFactory.CreateDropdown(bg, $"{idPrefix}Dropdown", out Dropdown dropdown, title, 14, (x) => { });
            UIFactory.SetLayoutElement(dropObj, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            foreach (string option in options)
                dropdown.options.Add(new Dropdown.OptionData(option));

            dropdown.value = currentIndex;

            Text desc = UIFactory.CreateLabel(bg, $"{idPrefix}Description", description, color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(desc.gameObject, flexibleWidth: 1);

            GameObject buttonsBg = UIFactory.CreateHorizontalGroup(bg, $"{idPrefix}ButtonsBG", false, true, true, true, UIConstants.SPACING, default);

            ButtonRef apply = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Apply", "Apply", UIConstants.greenButtonColor);
            RuntimeHelper.SetColorBlock(apply.Component, UIConstants.greenButtonColor, UIConstants.greenButtonHoverColor, UIConstants.greenButtonPressedColor);
            apply.OnClick += () => onApply(dropdown.value);
            UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef reset = UIFactory.CreateButton(buttonsBg, $"{idPrefix}Reset", "Reset", UIConstants.redButtonColor);
            RuntimeHelper.SetColorBlock(reset.Component, UIConstants.redButtonColor, UIConstants.redButtonHoverColor, UIConstants.redButtonPressedColor);
            reset.OnClick += () =>
            {
                dropdown.value = defaultIndex;
                onApply(defaultIndex);
            };
            UIFactory.SetLayoutElement(reset.Component.gameObject, 100, 25, 100, 25, 100, 25);

            return dropdown;
        }
    }
}