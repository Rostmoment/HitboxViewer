using HitboxViewer.Configs;
using HitboxViewer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


            #region Points per unit
            GameObject pointsPerUnitBg = UIFactory.CreateVerticalGroup(content, "PointsPerRadiusBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            InputFieldRef pointsPerUnitInput = UIFactory.CreateInputField(pointsPerUnitBg, "PointsPerRadiusInput", "Points per unit");
            pointsPerUnitInput.Text = config.PointsPerUnit.ToString();
            UIFactory.SetLayoutElement(pointsPerUnitInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text pointsPerUnitDescription = UIFactory.CreateLabel(pointsPerUnitBg, "PointsPerRadiusDescription", "Defines amount of points per unit, radius (and height for capsules) for rounded hitboxes");
            UIFactory.SetLayoutElement(pointsPerUnitDescription.gameObject, flexibleWidth: 1);

            GameObject pointsPerUnitButtonsBg = UIFactory.CreateHorizontalGroup(pointsPerUnitBg, "PointsPerRadiusButtonsBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyPointsPerRadius = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ApplyPointsPerRadius", "Apply", new Color(0, 0.39f, 0f));
            applyPointsPerRadius.OnClick += () =>
            {
                if (!float.TryParse(pointsPerUnitInput.Text, out float value))
                {
                    value = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT;
                    pointsPerUnitInput.Text = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                }

                config.PointsPerUnit = value;
            };
            UIFactory.SetLayoutElement(applyPointsPerRadius.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetPointsPerRadius = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ResetPointsPerRadius", "Reset", new Color(0.39f, 0f, 0f));
            resetPointsPerRadius.OnClick += () =>
            {
                pointsPerUnitInput.Text = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                config.PointsPerUnit = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT;
            };
            UIFactory.SetLayoutElement(resetPointsPerRadius.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

        }
    }
}
