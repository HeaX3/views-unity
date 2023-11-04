using System.Collections;
using UnityEngine;

namespace Views.UI
{
    public class DeviceSafeAreaObserver : MonoBehaviour
    {
        public delegate void Event();

        public static event Event SafeAreaChanged = delegate { };

        private static DeviceSafeAreaObserver _instance;

        private Rect _safeArea;

        private void OnEnable()
        {
            StartCoroutine(ApplyRoutine());
        }

        internal static void EnsureInstancePresent()
        {
            if (_instance) return;
            var gameObject = new GameObject("DeviceSafeAreaObserver");
            _instance = gameObject.AddComponent<DeviceSafeAreaObserver>();
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private IEnumerator ApplyRoutine()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForEndOfFrame();

                var safeArea = Screen.safeArea;
                if (_safeArea == safeArea) continue;
                _safeArea = safeArea;
                SafeAreaChanged();
            }
        }
    }
}