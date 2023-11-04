using UnityEngine;

namespace Views.UI
{
    public class DeviceSafeAreaOffset : AbstractDeviceSafeArea
    {
        [SerializeField] private RectTransform _rectTransform;

        private Vector2 _anchoredPosition;

        public RectTransform rectTransform => _rectTransform;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _anchoredPosition = rectTransform.anchoredPosition;
        }

        protected override void ApplyDeviceSafeAreas(Vector2Int min, Vector2Int max)
        {
            var rectTransform = this.rectTransform;

            var originalAnchoredPosition = _anchoredPosition;
            var anchoredPosition = originalAnchoredPosition;
            if (applyBottom) anchoredPosition.y = Mathf.Max(anchoredPosition.y, min.y);
            if (applyTop) anchoredPosition.y = Mathf.Min(anchoredPosition.y, max.y);
            if (applyLeft) anchoredPosition.x = Mathf.Max(anchoredPosition.x, min.x);
            if (applyRight) anchoredPosition.x = Mathf.Min(anchoredPosition.x, max.x);
            rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}