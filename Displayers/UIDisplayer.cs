using HarmonyLib;
using HitboxViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HitboxViewer.Displayers
{
    class UIDisplayer : BaseDisplayer
    {
        private static Dictionary<object, LineRenderer> renderers = new Dictionary<object, LineRenderer>();
        private static Dictionary<Type, UnityAction<UIDisplayer, object>> initialize = new Dictionary<Type, UnityAction<UIDisplayer, object>>()
        {
            {typeof(Button), (displayer, button) => displayer.Initialize((Button)button) },
            {typeof(Dropdown), (displayer, dropdown) => displayer.Initialize((Dropdown)dropdown) },
            {typeof(Slider), (displayer, slider) => displayer.Initialize((Slider)slider) }
        };
        public static List<UIDisplayer> all = new List<UIDisplayer>();

        public static void VisualizeGlobal(List<Component> list)
        {
            list.AddRange(GameObject.FindObjectsOfType<Button>());
            list.AddRange(GameObject.FindObjectsOfType<Dropdown>());
            list.AddRange(GameObject.FindObjectsOfType<Slider>());
            for (int i = 0; i < list.Count; i++)
                list[i].gameObject.GetOrAddComponent<UIDisplayer>().Visualize();
        }

        protected override void VirtualAwake()
        {
            base.VirtualAwake();
            all.Add(this);
        }
        protected override void VirtualOnDestroy()
        {
            base.VirtualOnDestroy();
            all.Remove(this);
        }
        protected override LineRenderer CreateLineRendered<T>(T collider, Dictionary<T, LineRenderer> renderers)
        {
            if (collider is not Button && collider is not Dropdown && collider is not Slider)
            {
                BasePlugin.Logger.LogWarning($"{collider.GetType()} is not UI element");
                return null;
            }
            Color color = HelpfulMethods.ColorFromHex(HitboxViewConfig.UIColor);
            LineRenderer line = base.CreateLineRendered(collider, renderers);
            Gradient gradient = new Gradient();
            gradient.SetKeys([new GradientColorKey(color, 0), new GradientColorKey(color, 1)],
                [new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)]);
            line.colorGradient = gradient;
            line.endColor = color;
            line.startColor = color;
            line.material = new Material(Shader.Find(HitboxViewConfig.ShaderHitboxName))
            {
                color = color
            };
            return line;
        }
        public override void Visualize()
        {
            base.Visualize();
            List<object> list = new List<object>();
            switch (BasePlugin.UIVisualize)
            {
                case UIVisualizationMode.Hide:
                    return;
                case UIVisualizationMode.Button:
                    list.AddRange(gameObject.GetComponents<Button>());
                    break;
                case UIVisualizationMode.Dropdown:
                    list.AddRange(gameObject.GetComponents<Dropdown>());
                    break;
                case UIVisualizationMode.Slider:
                    list.AddRange(gameObject.GetComponents<Slider>());
                    break;
                case UIVisualizationMode.All:
                    list.AddRange(gameObject.GetComponents<Button>());
                    list.AddRange(gameObject.GetComponents<Dropdown>());
                    list.AddRange(gameObject.GetComponents<Slider>());
                    break;
                default:
                    return;
            }
            list.Do(x => InitializeGlobal(x));
        }
        public override void InitializeGlobal<T>(T collider)
        {
            lineRenderer = CreateLineRendered(collider, renderers);
            if (lineRenderer == null || !lineRenderer.enabled || !initialize.TryGetValue(collider.GetType(), out var action))
                return;
            positions.Clear();
            action.Invoke(this, collider);
        }

        public static void UpdatePre()
        {
            UIDisplayer.all.RemoveAll(x => x == null);
            if (Input.GetKeyDown(HitboxViewConfig.ChangeUIVisualizeMode))
            {
                BasePlugin.UIVisualize = BasePlugin.UIVisualize.Next();
                UIDisplayer.HideAll();
                UIDisplayer.Show();
            }
            if (BasePlugin.UIVisualize == UIVisualizationMode.Hide)
                UIDisplayer.HideAll();
        }
        public static void HideAll()
        {
            renderers = ClearFromNull(renderers);
            LineRenderer[] lines = renderers.Values.ToArray();
            for (int i = 0; i < lines.Length; i++)
                lines[i].gameObject.SetActive(false);
        }
        public static void Show()
        {
            renderers = ClearFromNull(renderers);
            bool showButton = BasePlugin.UIVisualize == UIVisualizationMode.Button;
            bool showDropdown = BasePlugin.UIVisualize == UIVisualizationMode.Dropdown;
            bool showSlider = BasePlugin.UIVisualize == UIVisualizationMode.Slider;
            if (BasePlugin.UIVisualize == UIVisualizationMode.All)
            {
                showButton = true;
                showSlider = true;
                showDropdown = true;
            }
            foreach (var data in renderers)
            {
                try
                {
                    if (data.Key is Button)
                        data.Value.gameObject.SetActive(showButton);
                    if (data.Key is Slider)
                        data.Value.gameObject.SetActive(showSlider);
                    if (data.Key is Dropdown)
                        data.Value.gameObject.SetActive(showDropdown);
                }
                catch (NullReferenceException) { }
            }
        }
        public void Initialize(Button button)
        {
            DrawRectTransform(button.GetComponent<RectTransform>());
        }

        public void Initialize(Slider slider)
        {
            DrawRectTransform(slider.GetComponent<RectTransform>());
        }

        public void Initialize(Dropdown dropdown)
        {
            DrawRectTransform(dropdown.GetComponent<RectTransform>());
        }
    }
}
