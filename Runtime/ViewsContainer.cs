using System.Linq;
using Essentials;
using UnityEngine;

namespace Views
{
    public abstract class ViewsContainer : MonoBehaviour
    {
        private NavigationTree _navigationTree;
        protected View[] Views { get; private set; }

        #region Initializers

        public void Initialize()
        {
            OnBeforeInitialize();

            _navigationTree = BuildNavigationTree();
            Views = _navigationTree.Views;

            foreach (var view in Views)
            {
                if (!view) continue;
                view.gameObject.SetActive(true);
                view.Initialize(this);
                view.Opens += () => CloseViews(view);
                view.Opened += () => CloseViews(view);
                view.gameObject.SetActive(false);
            }

            foreach (var branch in _navigationTree.GetAllBranches())
            {
                branch.View.gameObject.SetActive(branch.Configuration.ShowOnAwake);
            }

            var go = gameObject;
            if (!go.activeSelf)
            {
                go.SetActive(true);
                go.SetActive(false);
            }

            OnInitialize();
        }

        #endregion

        #region Unity Control

        protected void OnEnable()
        {
            OnEnabled();
        }

        protected void OnDisable()
        {
            OnDisabled();
        }

        protected void OnDestroy()
        {
            OnDestroyed();
        }

        #endregion

        #region Public API

        public T GetView<T>() where T : View
        {
            return Views.OfType<T>().FirstOrDefault();
        }

        public View GetView(NamespacedKey viewId)
        {
            return Views.FirstOrDefault(v => v.Id == viewId);
        }

        public T GetView<T>(NamespacedKey viewId) where T : IGenericView
        {
            return Views.OfType<T>().FirstOrDefault(v => v.Id == viewId);
        }

        #endregion

        #region Virtual Members

        protected virtual void OnBeforeInitialize()
        {
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnEnabled()
        {
        }

        protected virtual void OnDisabled()
        {
        }

        protected virtual void OnDestroyed()
        {
        }

        #endregion

        #region Abstract Members

        protected abstract NavigationTree BuildNavigationTree();

        #endregion

        public void CloseViews()
        {
            foreach (var other in Views.Where(m => m.IsOpen))
            {
                other.Close();
            }
        }

        public void CloseViews(View except)
        {
            foreach (var other in Views.Where(m => m != except && m.IsOpen))
            {
                var branch = _navigationTree.GetBranch(other);
                if (branch.View == except || branch.Contains(except))
                {
                    continue;
                }

                other.Close();
            }
        }
    }
}