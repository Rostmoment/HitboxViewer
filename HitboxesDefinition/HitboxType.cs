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

namespace HitboxViewer.HitboxesDefinition
{
    class HitboxType
    {
        private const float DEFAULT_LINE_WIDTH = 0.1f;

        internal static List<HitboxType> all = new List<HitboxType>();

        #region constructor
        public HitboxType(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor, HitboxesFlags flags) : this(defaultKey, defaultStartColor, defaultEndColor, new BaseHitboxUI(), new HitboxTypeFlags(flags)) { }
        public HitboxType(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor, BaseHitboxUI ui, HitboxTypeFlags flags)
        {
            DefaultKeyBind = defaultKey;

            DefaultEndColor = defaultEndColor;
            DefaultStartColor = defaultStartColor;

            DefaultStartWidth = DEFAULT_LINE_WIDTH;
            DefaultEndWidth = DEFAULT_LINE_WIDTH;
            
            UI = ui;
            UI.hitboxType = this;
            Flags = flags;
            Flags.hitboxType = this;

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

        public BaseHitboxUI UI { get; }
        public HitboxTypeFlags Flags { get; }

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
    }
}
