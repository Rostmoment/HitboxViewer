using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Configs
{
    class SphereHitboxConfig : BaseHitboxConfig
    {
        public const float DEFAULT_POINTS_PER_UNIT = 100f;
        public const bool DEFAULT_USE_FIBONACCI_ALGORITHM = false;

        public SphereHitboxConfig(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor) : base(defaultKey, defaultStartColor, defaultEndColor)
        {
        }

        #region configs
        private ConfigEntry<float> pointsPerUnit;
        public float PointsPerUnit
        {
            get => pointsPerUnit.Value;
            set => pointsPerUnit.Value = value;
        }

        private ConfigEntry<bool> useFibonacci;
        public bool UseFibonacci
        {
            get => useFibonacci.Value;
            set => useFibonacci.Value = value;
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            pointsPerUnit = BasePlugin.Instance.Config.Bind<float>(
                hitboxType.Name,
                "Points per unit",
                DEFAULT_POINTS_PER_UNIT,
                "Defines amount of points per unit for rounded hitboxes\nFor Fibonacci algorithm unit is surface area of hitbox\nFor latitude-longitude algorithm unit is radius"
            );

            useFibonacci = BasePlugin.Instance.Config.Bind<bool>(
                hitboxType.Name,
                "Use Fibonacci algorithm",
                DEFAULT_USE_FIBONACCI_ALGORITHM,
                "If true, Fibonacci algorithm will be used instead of latitude-longitude algorithm for sphere. It is less accurate, but faster"
            );
        }
    }
}
