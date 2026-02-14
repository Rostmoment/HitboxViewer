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
    public class HitboxDefinition
    {

        internal static List<HitboxDefinition> all = new List<HitboxDefinition>();

        #region constructor
        public HitboxDefinition(string name, BaseHitboxConfig config, HitboxesFlags flags) : this(name, config, new BaseHitboxUI(), flags) { }
        public HitboxDefinition(string name, BaseHitboxConfig config, BaseHitboxUI ui, HitboxesFlags flags)
        {
            Name = name;
            
            UI = ui;
            UI.hitboxType = this;

            Flags = new HitboxTypeFlags(flags);
            Flags.hitboxType = this;

            Config = config;
            Config.hitboxType = this;

            all.Add(this);
        }
        #endregion

        #region properties

        public BaseHitboxUI UI { get; }
        public HitboxTypeFlags Flags { get; }
        public BaseHitboxConfig Config { get; }
        public string Name { get; private set; }
        #endregion

    }
}
