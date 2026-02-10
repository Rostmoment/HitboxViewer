using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HitboxViewer.HitboxesDefinition
{
    class BaseHitboxUI 
    {
        private GameObject content;
        public HitboxType hitboxType;

        public virtual GameObject BuildCategory(GameObject editorContent)
        {
            content = UIFactory.CreateVerticalGroup(editorContent, $"HitboxConfig{hitboxType.Category}", true, false, true, true, 4, default, new Color(0.05f, 0.05f, 0.05f));
            content.SetActive(false);

            GameObject bg = UIFactory.CreateHorizontalGroup(content, "TitleBG", true, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            Text title = UIFactory.CreateLabel(bg, $"Title{hitboxType.Category}", hitboxType.Category, TextAnchor.MiddleCenter, default, true, 17);
            UIFactory.SetLayoutElement(title.gameObject, minHeight: 30, minWidth: 200, flexibleWidth: 9999);

            GameObject buttons = UIFactory.CreateHorizontalGroup(content, "Buttons", true, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

            ButtonRef enableAll = UIFactory.CreateButton(buttons, "EnableAllFlags", "Enable all flags", new Color(0, 0.39f, 0f));
            UIFactory.SetLayoutElement(enableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            enableAll.OnClick += hitboxType.Flags.EnableAll;

            ButtonRef disableAll = UIFactory.CreateButton(buttons, "DisableAllFlags", "Disable all flags", new Color(0.39f, 0, 0f));
            UIFactory.SetLayoutElement(disableAll.Component.gameObject, minHeight: 35, flexibleHeight: 0, flexibleWidth: 9999);
            disableAll.OnClick += hitboxType.Flags.DisableAll;

            BuildSettings();

            return content;
        }

        public virtual void BuildSettings()
        {
            foreach (HitboxesFlags flag in FlagsExtensions.all)
            {
                if (!hitboxType.Flags.HasFlag(flag))
                    continue;

                GameObject bg = UIFactory.CreateVerticalGroup(content, "BG", false, true, true, true, 0, default, new Color(0.07f, 0.07f, 0.07f));

                GameObject toggleObject = UIFactory.CreateToggle(bg, $"Toggle{flag}", out Toggle toggle, out Text toggleText, new Color(0.1f, 0.1f, 0.1f));
                toggleText.text = "Enabled/Disabled";
                toggle.isOn = false;
                UIFactory.SetLayoutElement(toggleObject, 1, 25);

                Text description = UIFactory.CreateLabel(bg, $"Description{flag}", flag.GetDescription());
                UIFactory.SetLayoutElement(description.gameObject, flexibleWidth: 1);

                ButtonRef apply = UIFactory.CreateButton(bg, $"Apply{flag}", "Apply", new Color(0, 0.39f, 0f));
                apply.OnClick += () =>
                {
                    hitboxType.Flags.SetEnabled(toggle.isOn, flag);
                };
                UIFactory.SetLayoutElement(apply.Component.gameObject, 100, 25, 100, 25, 100, 25);
            }
        }
    }
}
