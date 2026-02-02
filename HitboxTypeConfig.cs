using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer
{
    internal class HitboxTypeConfig
    {
        public HitboxTypeConfig(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor, float defaultStartWidth, float defaultEndWidth)
        {
            DefaultKeyBind = defaultKey;

            DefaultEndColor = defaultEndColor;
            DefaultStartColor = defaultStartColor;

            DefaultStartWidth = defaultStartWidth;
            DefaultEndWidth = defaultEndWidth;
        }


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

        public float StartWidth => startWidthEntry.Value;
        private ConfigEntry<float> startWidthEntry;

        public float EndWidth => endWidthEntry.Value;
        private ConfigEntry<float> endWidthEntry;


        public void Initialize(string category)
        {
            startColorEntry = BasePlugin.Instance.Config.Bind<Color>(
                category,
                "Start Color",
                DefaultStartColor,
                $"Start color of the {category} hitbox outline"
            );

            endColorEntry = BasePlugin.Instance.Config.Bind<Color>(
                category,
                "End Color",
                DefaultEndColor,
                $"End color of the {category} hitbox outline"
            );

            keyBindEntry = BasePlugin.Instance.Config.Bind<KeyCode>(
                category,
                "Key Bind",
                DefaultKeyBind,
                $"Key bind to toggle the {category} hitbox outline"
            );

            startWidthEntry = BasePlugin.Instance.Config.Bind<float>(
                category,
                "Start Width",
                DefaultStartWidth,
                $"Start width of the {category} hitbox outline"
            );

            endWidthEntry = BasePlugin.Instance.Config.Bind<float>(
                category,
                "End Width",
                DefaultEndWidth,
                $"End width of the {category} hitbox outline"
            );
        }
    }
}
