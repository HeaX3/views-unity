using UnityEngine;

namespace Views.UI
{
    public class DeviceSafeArea : AbstractDeviceSafeArea
    {
        [SerializeField] private RectTransform _rectTransform;

        private Vector2 _offsetMin;
        private Vector2 _offsetMax;

        public RectTransform rectTransform => _rectTransform;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _offsetMin = rectTransform.offsetMin;
            _offsetMax = rectTransform.offsetMax;
        }

        protected override void ApplyDeviceSafeAreas(Vector2Int min, Vector2Int max)
        {
            var rectTransform = this.rectTransform;

            var originalMin = _offsetMin;
            var originalMax = _offsetMax;

            var apply = Screen.width > Screen.height ? applyLandscape : applyPortrait;

            var offsetMin = new Vector2(
                apply && applyLeft ? Mathf.Max(min.x, originalMin.x) : originalMin.x,
                apply && applyBottom ? Mathf.Max(min.y, originalMin.y) : originalMin.y
            );
            var offsetMax = new Vector2(
                apply && applyRight ? Mathf.Min(max.x, originalMax.x) : originalMax.x,
                apply && applyTop ? Mathf.Min(max.y, originalMax.y) : originalMax.y
            );

            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}