using BepInEx.Configuration;
using HitboxViewer.Configs;
using HitboxViewer.Flags;
using HitboxViewer.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HitboxViewer
{
    public class HitboxDefinition
    {
        internal static Dictionary<Type, HitboxDefinition> definitions = new Dictionary<Type, HitboxDefinition>();

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

        }
        #endregion

        #region properties

        public BaseHitboxUI UI { get; }
        public HitboxTypeFlags Flags { get; }
        public BaseHitboxConfig Config { get; }
        public string Name { get; private set; }
        #endregion


        #region definition methods
        public static void Define<T>(string name, BaseHitboxConfig config, HitboxesFlags flags)
        {
            Define(typeof(T), name, config, new BaseHitboxUI(), flags);
        }
        public static void Define(Type hitboxType, string name, BaseHitboxConfig config, HitboxesFlags flags)
        {
            Define(hitboxType, name, config, new BaseHitboxUI(), flags);
        }

        public static void Define<T>(string name, BaseHitboxConfig config, BaseHitboxUI ui, HitboxesFlags flags)
        {
            Define(typeof(T), name, config, ui, flags);
        }
        public static void Define(Type hitboxType, string name, BaseHitboxConfig config, BaseHitboxUI ui, HitboxesFlags flags) 
        { 
            Define(hitboxType, new HitboxDefinition(name, config, ui, flags));
        }

        public static void Define<T>(HitboxDefinition definition)
        {
            Define(typeof(T), definition);
        }
        public static void Define(Type hitboxType, HitboxDefinition definition)
        {
            definitions[hitboxType] = definition;
        }

        public static HitboxDefinition DefinitionOf<T>() => definitions[typeof(T)];
        public static HitboxDefinition DefinitionOf(Type type) => definitions[type]; 
        #endregion
    }
}
