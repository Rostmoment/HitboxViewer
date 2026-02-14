using HitboxViewer.Configs;
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
    class SphereHitboxUI : BaseHitboxUI
    {
        public override void BuildConfigs()
        {
            base.BuildConfigs();

            #region Fibonacci toggle
            GameObject fibonacciBg = UIFactory.CreateVerticalGroup(content, "FibonacciToggleBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));
            GameObject toggle = UIFactory.CreateToggle(fibonacciBg, "FibonacciToggle", out Toggle fibonacciToggle, out Text fibonacciText, new Color(0.1f, 0.1f, 0.1f));
            fibonacciText.text = "Use Fibonacci algorithm";
            fibonacciToggle.isOn = ((SphereHitboxConfig)hitboxType.Config).UseFibonacci;

            Text description = UIFactory.CreateLabel(fibonacciBg, $"FibonacciDescription", "If true, Fibonacci algorithm will be used instead of latitude-longitude algorithm for sphere. It is less accurate, but faster");
            UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

            GameObject fibonacciButtonsBg = UIFactory.CreateHorizontalGroup(fibonacciBg, "StartWidthButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyFibonacci = UIFactory.CreateButton(fibonacciButtonsBg, $"ApplyFibonacci", "Apply", new Color(0, 0.39f, 0f));
            applyFibonacci.OnClick += () =>
            {
                ((SphereHitboxConfig)hitboxType.Config).UseFibonacci = fibonacciToggle.isOn;
            };
            UIFactory.SetLayoutElement(applyFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetFibonacci = UIFactory.CreateButton(fibonacciButtonsBg, $"ResetFibonacci", "Reset", new Color(0.39f, 0f, 0f));
            resetFibonacci.OnClick += () =>
            {
                fibonacciToggle.isOn = SphereHitboxConfig.DEFAULT_USE_FIBONACCI_ALGORITHM;
                ((SphereHitboxConfig)hitboxType.Config).UseFibonacci = SphereHitboxConfig.DEFAULT_USE_FIBONACCI_ALGORITHM;
            };
            UIFactory.SetLayoutElement(resetFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

            #region Points per unit
            GameObject pointsPerUnitBg = UIFactory.CreateVerticalGroup(content, "PointsPerUnitBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            InputFieldRef pointsPerUnitInput = UIFactory.CreateInputField(pointsPerUnitBg, "PointsPerUnitInput", "Points per unit");
            pointsPerUnitInput.Text = ((SphereHitboxConfig)hitboxType.Config).PointsPerUnit.ToString();
            UIFactory.SetLayoutElement(pointsPerUnitInput.Component.gameObject, flexibleWidth: 9999, minHeight: 25);

            Text pointsPerUnitDescription = UIFactory.CreateLabel(
                pointsPerUnitBg,
                "PointsPerUnitDescription",
                $"Defines amount of points per unit for rounded hitboxes\n" +
                $"For Fibonacci algorithm unit is surface area of hitbox\n" +
                $"For latitude-longitude algorithm unit is radius\n" +
                $"Default: {SphereHitboxConfig.DEFAULT_POINTS_PER_UNIT}"
            );
            UIFactory.SetLayoutElement(pointsPerUnitDescription.gameObject, flexibleWidth: 1);

            GameObject pointsPerUnitButtonsBg = UIFactory.CreateHorizontalGroup(pointsPerUnitBg, "PointsPerUnitButtonsBg", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyPointsPerUnit = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ApplyPointsPerUnit", "Apply", new Color(0, 0.39f, 0f));
            applyPointsPerUnit.OnClick += () =>
            {
                if (!float.TryParse(pointsPerUnitInput.Text, out float value))
                {
                    value = SphereHitboxConfig.DEFAULT_POINTS_PER_UNIT;
                    pointsPerUnitInput.Text = SphereHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                }

                ((SphereHitboxConfig)hitboxType.Config).PointsPerUnit = value;
            };
            UIFactory.SetLayoutElement(applyPointsPerUnit.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetPointsPerUnit = UIFactory.CreateButton(pointsPerUnitButtonsBg, "ResetPointsPerUnit", "Reset", new Color(0.39f, 0f, 0f));
            resetPointsPerUnit.OnClick += () =>
            {
                pointsPerUnitInput.Text = SphereHitboxConfig.DEFAULT_POINTS_PER_UNIT.ToString();
                ((SphereHitboxConfig)hitboxType.Config).PointsPerUnit = SphereHitboxConfig.DEFAULT_POINTS_PER_UNIT;
            };
            UIFactory.SetLayoutElement(resetPointsPerUnit.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion

        }
    }
}
