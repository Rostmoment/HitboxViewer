using HitboxViewer.Flags;
using HitboxViewer.HitboxesDefinition;
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
        protected static Dictionary<Component, BaseDisplayer> instances = new Dictionary<Component, BaseDisplayer>();
        public static BaseDisplayer GetOrAdd(Component component, Type displayerType)
        {
            if (instances.TryGetValue(component, out BaseDisplayer diplayer))
                return diplayer;

            diplayer = (BaseDisplayer)component.gameObject.AddComponent(displayerType);
            instances[component] = diplayer;
            return diplayer;
        }
        #endregion

        #region fields and properties
        protected Vector3[] points;
        protected LineRenderer lineRenderer;
        protected GameObject parentObject;

        private bool Hidden
        {
            set => lineRenderer.enabled = !value;
            get => !lineRenderer.enabled;
        }

        public abstract HitboxesFlags HitboxFlags { get; }

        #endregion

        #region position setting
        public void SetPositions(List<Vector3> vectors) => SetPositions(vectors.ToArray());
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
        protected T target;
        public bool AnyFlagEnabled
        {
            get
            {
                HitboxType type = HitboxViewerConfig.InfoOf<T>();
                foreach (HitboxesFlags flag in FlagsExtensions.all)
                {
                    if (type.Flags.IsEnabled(flag))
                        return true;
                }
                return false;
            }
        }

        protected override bool ShouldBeDisplayed() => AnyFlagEnabled;

        protected override void VirtualAwake()
        {
            CreateLineRenderer();
            target = GetComponent<T>();
            instances[target] = this;
        }

        protected override void VirtualUpdate()
        {
            if (target.IsNullOrDestroyed())
            {
                Destroy(this);
                return;
            }
        }

        protected void CreateLineRenderer()
        {
            if (!lineRenderer.IsNullOrDestroyed())
                return;

            if (parentObject.IsNullOrDestroyed())
            {
                parentObject = new GameObject($"[HitboxViewer] {typeof(T).Name} Displayer");
                parentObject.transform.SetParent(gameObject.transform);
            }

            HitboxType config = HitboxViewerConfig.InfoOf<T>();
            lineRenderer = parentObject.AddComponent<LineRenderer>();

            lineRenderer.material = new Material(Shader.Find(HitboxViewerConfig.ShaderName))
            {
                color = Color.white
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys([new GradientColorKey(config.StartColor, 0), new GradientColorKey(config.EndColor, 1)],  [new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)]);

            lineRenderer.colorGradient = gradient;
            lineRenderer.startColor = config.StartColor;
            lineRenderer.endColor = config.EndColor;

            lineRenderer.startWidth = config.StartWidth;

            lineRenderer.endWidth = config.EndWidth;
            lineRenderer.loop = false;
            lineRenderer.useWorldSpace = true;

            Hide();
        }

    }
}
