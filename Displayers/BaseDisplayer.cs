using HitboxViewer.Configs;
using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniverseLib.Utility;

namespace HitboxViewer.Displayers
{
    public abstract class BaseDisplayer : MonoBehaviour
    {
        #region virtual unity methods
        private void Update() => VirtualUpdate();
        protected virtual void VirtualUpdate() { }

        private void Awake() => VirtualAwake();
        protected virtual void VirtualAwake() { }
        #endregion

        #region creation of line renderer and instance management
        protected static Dictionary<Component, BaseDisplayer> displayers = new Dictionary<Component, BaseDisplayer>();

        public static T GetOrAdd<T>(Component component) where T : BaseDisplayer => (T)GetOrAdd(component, typeof(T));
        public static BaseDisplayer GetOrAdd(Component component, Type displayerType)
        {
            if (displayers.TryGetValue(component, out BaseDisplayer diplayer))
                return diplayer;

            GameObject parentObject = new GameObject("HitboxViewerObject");
            parentObject.transform.SetParent(component.transform);
            diplayer = (BaseDisplayer)parentObject.AddComponent(displayerType);
            diplayer.target = component;

            displayers[component] = diplayer;

            return diplayer;
        }
        #endregion

        #region fields and properties
        protected Vector3[] points;
        protected LineRenderer lineRenderer;
        protected Component target;

        private bool Hidden
        {
            set => lineRenderer.enabled = !value;
            get => !lineRenderer.enabled;
        }

        public abstract HitboxesFlags HitboxFlags { get; }

        #endregion

        #region position setting
        public void SetPositions(params Vector3[] positions)
        {
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            points = positions;
        }
        #endregion

        #region visualizer control
        public virtual void Hide()
        {
            Hidden = true;
        }
        public virtual void Show()
        {
            Hidden = false;
        }

        protected abstract void _Visualize();

        public void Visualize()
        {
            if (target.IsNullOrDestroyed())
            {
                Destroy(this);
                return;
            }


            if (ShouldBeDisplayed())
            {
                if (ShouldBeUpdated())
                    _Visualize();
                Show();
            }
            else
                Hide();
        }

        protected abstract bool ShouldBeDisplayed();
        public bool ShouldBeUpdated() => _ShouldBeUpdated() || points == null || points.Length == 0;
        public virtual bool _ShouldBeUpdated() => true;
        #endregion
    }
    public abstract class BaseDisplayer<T> : BaseDisplayer where T : Component
    {
        protected T GenericTarget => (T)target;
        public bool AnyFlagEnabled
        {
            get
            {
                HitboxDefinition type = HitboxViewerConfig.DefinitionOf<T>();
                foreach (HitboxesFlags flag in FlagsExtensions.all)
                {
                    if (type.Flags.IsEnabled(flag) && HitboxFlags.HasFlag(flag))
                        return true;
                }
                return false;
            }
        }

        protected override bool ShouldBeDisplayed() => AnyFlagEnabled;

        protected override void VirtualAwake()
        {
            CreateLineRenderer();
        }

        protected override void VirtualUpdate()
        {
        }

        protected void CreateLineRenderer()
        {
            if (!lineRenderer.IsNullOrDestroyed())
                return;

            HitboxDefinition definition = HitboxViewerConfig.DefinitionOf<T>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();

            lineRenderer.material = new Material(Shader.Find(HitboxViewerConfig.ShaderName))
            {
                color = Color.white
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys([new GradientColorKey(definition.Config.StartColor, 0), new GradientColorKey(definition.Config.EndColor, 1)],  [new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)]);

            lineRenderer.colorGradient = gradient;
            lineRenderer.startColor = definition.Config.StartColor;
            lineRenderer.endColor = definition.Config.EndColor;

            lineRenderer.startWidth = definition.Config.StartWidth;

            lineRenderer.endWidth = definition.Config.EndWidth;
            lineRenderer.loop = false;
            lineRenderer.useWorldSpace = true;

            Hide();
        }

    }
}
