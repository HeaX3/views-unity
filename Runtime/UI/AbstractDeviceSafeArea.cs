using System;
using UnityEngine;

namespace Views.UI
{
    public abstract class AbstractDeviceSafeArea : MonoBehaviour
    {
        [SerializeField] private bool _applyTop = true;
        [SerializeField] private bool _applyRight = true;
        [SerializeField] private bool _applyBottom = true;
        [SerializeField] private bool _applyLeft = true;
        [SerializeField] private bool _applyLandscape = true;
        [SerializeField] private bool _applyPortrait = true;

        private Canvas canvas;
        private float _scale;
        private bool _initialized;

        private Vector2Int _calculatedMin;
        private Vector2Int _calculatedMax;

        protected bool applyTop => _applyTop;
        protected bool applyRight => _applyRight;
        protected bool applyBottom => _applyBottom;
        protected bool applyLeft => _applyLeft;
        protected bool applyLandscape => _applyLandscape;
        protected bool applyPortrait => _applyPortrait;

        private void Initialize()
        {
            _initialized = true;
            DeviceSafeAreaObserver.EnsureInstancePresent();
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        private void OnEnable()
        {
            canvas = GetComponentInParent<Canvas>().rootCanvas;
            if (!_initialized) Initialize();
            DeviceSafeAreaObserver.SafeAreaChanged += ApplyDeviceSafeAreas;
            ApplyDeviceSafeAreas();
        }

        private void LateUpdate()
        {
            if (Math.Abs(_scale - canvas.scaleFactor) < 0.001f) return;
            _scale = canvas.scaleFactor;
            ApplyDeviceSafeAreas();
        }

        private void OnDisable()
        {
            DeviceSafeAreaObserver.SafeAreaChanged -= ApplyDeviceSafeAreas;
        }

        public void ApplyDeviceSafeAreas()
        {
            if (!canvas) return;
            var safeArea = Screen.safeArea;
            var scale = canvas.scaleFactor;

            var safeAreaMin = safeArea.min;
            var safeAreaMax = safeArea.max;
            var screenSize = new Vector2Int(Screen.width, Screen.height);
            var calculatedMin = PixelPerfect(safeAreaMin / scale);
            var calculatedMax = PixelPerfect(-(screenSize - safeAreaMax) / scale);

            _calculatedMin = calculatedMin;
            _calculatedMax = calculatedMax;

            UpdateState();
        }

        public void UpdateState()
        {
            ApplyDeviceSafeAreas(_calculatedMin, _calculatedMax);
        }

        private Vector2Int PixelPerfect(Vector2 vector)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }

        protected abstract void ApplyDeviceSafeAreas(Vector2Int min, Vector2Int max);
    }
}