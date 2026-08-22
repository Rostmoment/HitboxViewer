using HitboxViewer.Configs;
using HitboxViewer.Constants;
using HitboxViewer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HitboxViewer.UI
{
    public class RoundedHitboxUI : BaseHitboxUI
    {
        public override void BuildConfigs()
        {
            base.BuildConfigs();
            RoundedHitboxConfig config = (RoundedHitboxConfig)hitboxType.Config;

            CreateFloatSetting(
                "PointsPerUnit",
                "Points Per Unit",
                "Defines amount of points per unit, radius (and height for capsules) for rounded hitboxes",
                config.PointsPerUnit,
                RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT,
                value => config.PointsPerUnit = value);
        }
    }
}