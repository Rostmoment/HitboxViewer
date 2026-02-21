using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace HitboxViewer.Configs
{
    public class BaseHitboxConfig
    {
        private const float DEFAULT_LINE_WIDTH = 0.1f;
        public HitboxDefinition hitboxType;

        public BaseHitboxConfig(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor)
        {

            DefaultKeyBind = defaultKey;

            DefaultEndColor = defaultEndColor;
            DefaultStartColor = defaultStartColor;

            DefaultStartWidth = DEFAULT_LINE_WIDTH;
            DefaultEndWidth = DEFAULT_LINE_WIDTH;
        }

        
        #region configs

        public Color DefaultStartColor { get; }
        public Color DefaultEndColor { get; }

        public KeyCode DefaultKeyBind { get; }

        public float DefaultStartWidth { get; }
        public float DefaultEndWidth { get; }

        public KeyboardShortcut KeyboardShortcut
        {
            get => keyboardShortcut.Value;
            set => keyboardShortcut.Value = value;
        }
        private ConfigEntry<KeyboardShortcut> keyboardShortcut;

        public Color StartColor
        {
            get => startColorEntry.Value;
            set => startColorEntry.Value = value;
        }
        private ConfigEntry<Color> startColorEntry;

        public Color EndColor
        {
            get => endColorEntry.Value;
            set => endColorEntry.Value = value;
        }
        private ConfigEntry<Color> endColorEntry;


        public float StartWidth
        {
            get => startWidthEntry.Value;
            set => startWidthEntry.Value = value;
        }
        private ConfigEntry<float> startWidthEntry;

        public float EndWidth
        {
            get => endWidthEntry.Value;
            set => endWidthEntry.Value = value;
        }
        private ConfigEntry<float> endWidthEntry;

        #endregion

        public virtual void Initialize()
        {

            startColorEntry = BasePlugin.Instance.Config.Bind(
                hitboxType.Name,
                "Start Color",
                DefaultStartColor,
                $"Start color of the {hitboxType.Name} hitbox outline"
            );

            endColorEntry = BasePlugin.Instance.Config.Bind(
                hitboxType.Name,
                "End Color",
                DefaultEndColor,
                $"End color of the {hitboxType.Name} hitbox outline"
            );

            keyboardShortcut = BasePlugin.Instance.Config.Bind(
                hitboxType.Name,
                "Key Bind",
                new KeyboardShortcut(DefaultKeyBind),
                $"Key bind to toggle the {hitboxType.Name} hitbox outline"
            );

            startWidthEntry = BasePlugin.Instance.Config.Bind(
                hitboxType.Name,
                "Start Width",
                DefaultStartWidth,
                $"Start width of the {hitboxType.Name} hitbox outline"
            );

            endWidthEntry = BasePlugin.Instance.Config.Bind(
                hitboxType.Name,
                "End Width",
                DefaultEndWidth,
                $"End width of the {hitboxType.Name} hitbox outline"
            );
        }
    }
}
