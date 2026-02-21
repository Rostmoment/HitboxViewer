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
    public class RoundedHitboxConfig : BaseHitboxConfig
    {
        public const float DEFAULT_POINTS_PER_UNIT = 100f;
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

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            pointsPerUnit = BasePlugin.Instance.Config.Bind<float>(
                hitboxType.Name,
                "Points per unit",
                DEFAULT_POINTS_PER_UNIT,
                "Defines amount of points per unit, radius (and height for capsules) for rounded hitboxes"
            );

        }
    }
}
