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
    class RoundedHitboxConfig : BaseHitboxConfig
    {
        public const float DEFAULT_POINTS_PER_UNIT = 100f;
        public const RoundedHitboxAlgorithms DEFAULT_ALGORITHM = RoundedHitboxAlgorithms.LatitudeLongitude;

        public RoundedHitboxConfig(KeyCode defaultKey, Color defaultStartColor, Color defaultEndColor) : base(defaultKey, defaultStartColor, defaultEndColor)
        {
        }

        #region configs
        private ConfigEntry<float> pointsPerUnit;
        public float PointsPerUnit
        {
            get => pointsPerUnit.Value;
            set => pointsPerUnit.Value = value;
        }

        private ConfigEntry<RoundedHitboxAlgorithms> algorithm;
        public RoundedHitboxAlgorithms Algorithm
        {
            get => algorithm.Value;
            set => algorithm.Value = value;
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            pointsPerUnit = BasePlugin.Instance.Config.Bind<float>(
                hitboxType.Name,
                "Points per unit",
                DEFAULT_POINTS_PER_UNIT,
                "Defines amount of points per unit for rounded hitboxes\nFor Fibonacci algorithm unit is surface area of hitbox\nFor other unit is radius"
            );

            algorithm = BasePlugin.Instance.Config.Bind<RoundedHitboxAlgorithms>(
                hitboxType.Name,
                "Use Fibonacci algorithm",
                DEFAULT_ALGORITHM,
                "Defines what algorithm will be used for drawing rounded hitboxes"
            );
        }
    }
}
