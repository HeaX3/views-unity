using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class DeviceSafeAreaSpacer : AbstractDeviceSafeArea
    {
        [SerializeField] private LayoutElement _layoutElement = null;

        public LayoutElement layoutElement => _layoutElement;

        protected override void ApplyDeviceSafeAreas(Vector2Int min, Vector2Int max)
        {
            var width = (applyLeft ? min.x : 0) + (applyRight ? max.x : 0);
            var height = (applyBottom ? min.y : 0) + (applyTop ? max.y : 0);
            layoutElement.minWidth = width;
            layoutElement.minHeight = height;
            layoutElement.preferredWidth = height;
            layoutElement.preferredHeight = height;
        }
    }
}