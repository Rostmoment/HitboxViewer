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
    class RoundedHitboxUI : BaseHitboxUI
    {
        public override void BuildConfigs()
        {
            base.BuildConfigs();

            #region Algorithm dropdown
            GameObject algorithmBg = UIFactory.CreateVerticalGroup(content, "AlgorithmBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            GameObject drop = UIFactory.CreateDropdown(algorithmBg, "AlgorithmDropdown", out Dropdown dropdown, "Algorithm", 14, (x) => { });
            UIFactory.SetLayoutElement(drop, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            foreach (RoundedHitboxAlgorithms algorithm in RoundedHitboxAlgorithmExtensions.all)
                dropdown.options.Add(new Dropdown.OptionData($"{algorithm} ({algorithm.GetDescription()})"));

            Text description = UIFactory.CreateLabel(algorithmBg, $"AlgorithmDescription", "Defines what algorithm will be used for drawing rounded hitboxes");
            UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);


            GameObject algorithmsButtonsBg = UIFactory.CreateHorizontalGroup(algorithmBg, "AlgoritgmButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyFibonacci = UIFactory.CreateButton(algorithmsButtonsBg, $"ApplyAlgorithm", "Apply", new Color(0, 0.39f, 0f));
            applyFibonacci.OnClick += () =>
            {
                ((RoundedHitboxConfig)hitboxType.Config).Algorithm = (RoundedHitboxAlgorithms)dropdown.value;
            };
            UIFactory.SetLayoutElement(applyFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetFibonacci = UIFactory.CreateButton(algorithmsButtonsBg, $"ResetAlgorithm", "Reset", new Color(0.39f, 0f, 0f));
            resetFibonacci.OnClick += () =>
            {
                dropdown.value = (int)RoundedHitboxConfig.DEFAULT_ALGORITHM;
                ((RoundedHitboxConfig)hitboxType.Config).Algorithm = RoundedHitboxConfig.DEFAULT_ALGORITHM;
            };
            UIFactory.SetLayoutElement(resetFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region Points per unit
            GameObject pointsPerUnitBg = UIFactory.CreateVerticalGroup(content, "PointsPerUnitBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            InputFieldRef pointsPerUnitInput = UIFactory.CreateInputField(pointsPerUnitBg, "PointsPerUnitInput", "Points per unit");
            pointsPerUnitInput.Text = ((RoundedHitboxConfig)hitboxType.Config).PointsPerUnit.ToString();
            UIFactory.SetLayoutElement(pointsPerUnitInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text pointsPerUnitDescription = UIFactory.CreateLabel(
                pointsPerUnitBg,
                "PointsPerUnitDescription",
                $"Defines amount of points per unit for rounded hitboxes\n" +
                $"For Fibonacci algorithm unit is surface area of hitbox\n" +
                $"For other unit is radius\n" +
                $"Default: {RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT}"
            );
            UIFactory.SetLayoutElement(pointsPerUnitDescription.gameObject, flexibleWidth: 1);

            GameObject pointsPerUnitButtonsBg = UIFactory.CreateHorizontalGroup(pointsPerUnitBg, "PointsPerUnitButtonsBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyPointsPerUnit = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ApplyPointsPerUnit", "Apply", new Color(0, 0.39f, 0f));
            applyPointsPerUnit.OnClick += () =>
            {
                if (!float.TryParse(pointsPerUnitInput.Text, out float value))
                {
                    value = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT;
                    pointsPerUnitInput.Text = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                }

                ((RoundedHitboxConfig)hitboxType.Config).PointsPerUnit = value;
            };
            UIFactory.SetLayoutElement(applyPointsPerUnit.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetPointsPerUnit = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ResetPointsPerUnit", "Reset", new Color(0.39f, 0f, 0f));
            resetPointsPerUnit.OnClick += () =>
            {
                pointsPerUnitInput.Text = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                ((RoundedHitboxConfig)hitboxType.Config).PointsPerUnit = RoundedHitboxConfig.DEFAULT_POINTS_PER_UNIT;
            };
            UIFactory.SetLayoutElement(resetPointsPerUnit.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

        }
    }
}
