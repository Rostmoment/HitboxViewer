using BepInEx.Configuration;
using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HitboxViewer.HitboxTypes
{
    internal class BaseHitboxType
    {
        internal static List<BaseHitboxType> all = new List<BaseHitboxType>();

        #region constructor
        public BaseHitboxType(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor, float defaultStartWidth, float defaultEndWidth, HitboxesFlags flags)
        {
            DefaultKeyBind = defaultKey;

            DefaultEndColor = defaultEndColor;
            DefaultStartColor = defaultStartColor;

            DefaultStartWidth = defaultStartWidth;
            DefaultEndWidth = defaultEndWidth;

            PotentionalFlags = flags;
            EnabledFlags = HitboxesFlags.None;

            all.Add(this);
        }
        #endregion

        #region properties
        public Color DefaultStartColor { get; }
        public Color DefaultEndColor { get; }

        public KeyCode DefaultKeyBind { get; }

        public float DefaultStartWidth { get; }
        public float DefaultEndWidth { get; }

        public KeyCode KeyBind => keyBindEntry.Value;
        private ConfigEntry<KeyCode> keyBindEntry;

        public Color StartColor => startColorEntry.Value;
        private ConfigEntry<Color> startColorEntry;

        public Color EndColor => endColorEntry.Value;
        private ConfigEntry<Color> endColorEntry;

        public Color AverageColor => Color.Lerp(StartColor, EndColor, 0.5f);

        public float StartWidth => startWidthEntry.Value;
        private ConfigEntry<float> startWidthEntry;

        public float EndWidth => endWidthEntry.Value;
        private ConfigEntry<float> endWidthEntry;

        public HitboxesFlags PotentionalFlags { get; }
        public HitboxesFlags EnabledFlags { get; private set; }

        public string Category { get; private set; }
        private bool initialized = false;
        #endregion

        public void Initialize(string category)
        {
            if (initialized)
                return;

            Category = category;
            initialized = true;

            startColorEntry = BasePlugin.Instance.Config.Bind(
                category,
                "Start Color",
                DefaultStartColor,
                $"Start color of the {category} hitbox outline"
            );

            endColorEntry = BasePlugin.Instance.Config.Bind(
                category,
                "End Color",
                DefaultEndColor,
                $"End color of the {category} hitbox outline"
            );

            keyBindEntry = BasePlugin.Instance.Config.Bind(
                category,
                "Key Bind",
                DefaultKeyBind,
                $"Key bind to toggle the {category} hitbox outline"
            );

            startWidthEntry = BasePlugin.Instance.Config.Bind(
                category,
                "Start Width",
                DefaultStartWidth,
                $"Start width of the {category} hitbox outline"
            );

            endWidthEntry = BasePlugin.Instance.Config.Bind(
                category,
                "End Width",
                DefaultEndWidth,
                $"End width of the {category} hitbox outline"
            );
        }

        public void Enable(HitboxesFlags flag)
        {
            if (!EnabledFlags.HasFlag(flag))
                EnabledFlags |= flag;
        }

        public void Disable(HitboxesFlags flag)
        {
            if (EnabledFlags.HasFlag(flag))
                EnabledFlags &= ~flag;
        }

        public void EnableAll()
        {
            EnabledFlags = PotentionalFlags;
        }
        public void DisableAll()
        {
            EnabledFlags = HitboxesFlags.None;
        }
        
        public void SetAll(bool enabled)
        {
            if (enabled)
                EnableAll();
            else
                DisableAll();
        }
        public void SetEnabled(bool enabled, HitboxesFlags flag)
        {
            if (enabled)
                Enable(flag);
            else
                Disable(flag);
        }
        public bool IsEnabled(HitboxesFlags flag) => EnabledFlags.HasFlag(flag);
        public bool HasFlag(HitboxesFlags flag) => PotentionalFlags.HasFlag(flag);

        public void BuildSettings(GameObject content)
        {
            foreach (HitboxesFlags flag in FlagsExtensions.all)
            {
                if (!HasFlag(flag))
                    continue;

                GameObject bg = UIFactory.CreateVerticalGroup(content, "BG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

                GameObject toggleObject = UIFactory.CreateToggle(bg, $"Toggle{flag}", out Toggle toggle, out Text toggleText, new Color(0.1f, 0.1f, 0.1f));
                toggleText.color = new Color(0.8f, 0.8f, 0.8f);
                toggleText.text = "Enabled/Disabled";
                toggle.isOn = false;
                UIFactory.SetLayoutElement(toggleObject, 1, 25);

                Text description = UIFactory.CreateLabel(bg, $"Description{flag}", flag.GetDescription());
                UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

                GameObject horizontal = UIFactory.CreateHorizontalGroup(bg, "ApplyButtonBG", true, true, true, true, 0, default, Color.clear);

                ButtonRef apply = UIFactory.CreateButton(horizontal, $"Apply{flag}", "Apply", new Color(0, 0.39f, 0f));
                apply.OnClick += () =>
                {
                    BasePlugin.Logger.LogDebug($"Setting {flag} for {Category} to {toggle.isOn}");
                    SetEnabled(toggle.isOn, flag);
                };
                UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);
                
            }
        }
    }
}
