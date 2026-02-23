using HitboxViewer.Configs;
using HitboxViewer.Constants;
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
            GameObject pointsPerUnitBg = CreateConfigBlockBG("PointsPerRadiusBg");

            Text title = UIFactory.CreateLabel(pointsPerUnitBg, "PointsPerUnit", "Points Per Unit");
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            InputFieldRef pointsPerUnitInput = UIFactory.CreateInputField(pointsPerUnitBg, "PointsPerRadiusInput", "Points per unit");
            pointsPerUnitInput.Text = config.PointsPerUnit.ToString();
            UIFactory.SetLayoutElement(pointsPerUnitInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text pointsPerUnitDescription = UIFactory.CreateLabel(pointsPerUnitBg, "PointsPerRadiusDescription", "Defines amount of points per unit, radius (and height for capsules) for rounded hitboxes", color: UIConstants.configDescriptionColor);
            UIFactory.SetLayoutElement(pointsPerUnitDescription.gameObject, flexibleWidth: 1);

            GameObject pointsPerUnitButtonsBg = UIFactory.CreateHorizontalGroup(pointsPerUnitBg, "PointsPerRadiusButtonsBg", false, true, true, true, 0, default);

            ButtonRef applyPointsPerRadius = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ApplyPointsPerRadius", "Apply", UIConstants.greenButtonColor);
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

            ButtonRef resetPointsPerRadius = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ResetPointsPerRadius", "Reset", UIConstants.redButtonColor);
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
