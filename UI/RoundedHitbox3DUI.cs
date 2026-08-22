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
    public class RoundedHitbox3DUI : RoundedHitboxUI
    {
        public override void BuildConfigs()
        {
            base.BuildConfigs();

            RoundedHitboxConfig3D config = (RoundedHitboxConfig3D)hitboxType.Config;

            IEnumerable<string> algorithmOptions = RoundedHitboxAlgorithmExtensions.all
                .Select(algorithm => $"{algorithm} ({algorithm.Description})");

            CreateDropdownSetting(
                "Algorithm",
                "Drawing Algorithm",
                "Defines what algorithm will be used for drawing rounded hitboxes",
                algorithmOptions,
                (int)config.Algorithm,
                (int)config.DefaultAlgorithm,
                value => config.Algorithm = (RoundedHitboxAlgorithm)value);
        }
    }
}