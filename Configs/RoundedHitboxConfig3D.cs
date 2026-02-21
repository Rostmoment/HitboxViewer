using BepInEx.Configuration;
using HitboxViewer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Configs
{
    public class RoundedHitboxConfig3D : RoundedHitboxConfig
    {
        public RoundedHitboxAlgorithm DefaultAlgorithm { get; }

        public RoundedHitboxConfig3D(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor, RoundedHitboxAlgorithm defaultAlgorithm) : base(defaultKey, defaultStartColor, defaultEndColor)
        {
            DefaultAlgorithm = defaultAlgorithm;
        }

        private ConfigEntry<RoundedHitboxAlgorithm> algorithm;
        public RoundedHitboxAlgorithm Algorithm
        {
            get => algorithm.Value;
            set => algorithm.Value = value;
        }
        public override void Initialize()
        {
            base.Initialize();

            algorithm = BasePlugin.Instance.Config.Bind<RoundedHitboxAlgorithm>(
                hitboxType.Name,
                "Sphere draw algorithm",
                DefaultAlgorithm,
                "Defines what algorithm will be used for drawing rounded hitboxes"
            );
        }


    }
}
