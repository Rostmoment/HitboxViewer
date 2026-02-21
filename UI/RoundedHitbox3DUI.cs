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
    public class RoundedHitbox3DUI : RoundedHitboxUI
    {
        public override void BuildConfigs()
        {
            base.BuildConfigs();

            RoundedHitboxConfig3D config = (RoundedHitboxConfig3D)hitboxType.Config;

            #region Algorithm dropdown
            GameObject algorithmBg = UIFactory.CreateVerticalGroup(content, "AlgorithmBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            GameObject drop = UIFactory.CreateDropdown(algorithmBg, "AlgorithmDropdown", out Dropdown dropdown, "Algorithm", 14, (x) => { });
            UIFactory.SetLayoutElement(drop, minHeight: 25, minWidth: 110, flexibleWidth: 999);

            foreach (RoundedHitboxAlgorithm algorithm in RoundedHitboxAlgorithmExtensions.all)
                dropdown.options.Add(new Dropdown.OptionData($"{algorithm} ({algorithm.GetDescription()})"));

            dropdown.value = (int)config.Algorithm;

            Text description = UIFactory.CreateLabel(algorithmBg, $"AlgorithmDescription", "Defines what algorithm will be used for drawing rounded hitboxes");
            UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);


            GameObject algorithmsButtonsBg = UIFactory.CreateHorizontalGroup(algorithmBg, "AlgoritgmButtonsBG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef applyFibonacci = UIFactory.CreateButton(algorithmsButtonsBg, $"ApplyAlgorithm", "Apply", new Color(0, 0.39f, 0f));
            applyFibonacci.OnClick += () =>
            {
                config.Algorithm = (RoundedHitboxAlgorithm)dropdown.value;
            };
            UIFactory.SetLayoutElement(applyFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);

            ButtonRef resetFibonacci = UIFactory.CreateButton(algorithmsButtonsBg, $"ResetAlgorithm", "Reset", new Color(0.39f, 0f, 0f));
            resetFibonacci.OnClick += () =>
            {
                dropdown.value = (int)config.DefaultAlgorithm;
                config.Algorithm = config.DefaultAlgorithm;
            };
            UIFactory.SetLayoutElement(resetFibonacci.Component.gameObject, 100, 25, 100, 25, 100, 25);
            #endregion
        }
    }
}
