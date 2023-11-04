using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Essentials;
using RSG;
using UnityEngine;
using UnityEngine.Events;

namespace Views
{
    /// <summary>
    /// Use this for overall menu control for a specific menu
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        public delegate void ViewEvent();

        public event ViewEvent Opens = delegate { };
        public event ViewEvent Opened = delegate { };
        public event ViewEvent Closes = delegate { };
        public event ViewEvent Closed = delegate { };

        public abstract NamespacedKey Id { get; }

        [SerializeField] private UnityEvents _events = null;

        private bool _isOpen = false;
        private bool _isActive = false;

        public bool IsOpen => _isOpen;
        public bool IsSuspended => IsOpen && !_isActive;

        #region Initializers

        /// <summary>
        /// Initializes once when scene is loaded
        /// </summary>
        public void Initialize(ViewsContainer viewsContainer)
        {
            OnInitialize(viewsContainer);
            OnInitialize();
            gameObject.SetActive(false);
        }

        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Initialize once when scene is load, can be overloaded
        /// </summary>
        protected virtual void OnInitialize(ViewsContainer viewsContainer)
        {
        }

        #endregion

        #region Unity Control

        protected void OnDestroy()
        {
            OnDestroyed();
        }

        protected virtual void OnDestroyed()
        {
        }

        #endregion

        #region Public API

        protected IPromise Open()
        {
            return new Promise((resolve, reject) =>
            {
                _isOpen = true;

                Opens();
                _events.onOpens.Invoke();

                gameObject.SetActive(true);
                OnBeforeOpen();

                var promise = PrepareOpen();
                if (promise != null)
                {
                    promise.Then(() =>
                    {
                        ExecuteOpened();
                        resolve();
                    }).Catch(e =>
                    {
                        Debug.LogError(e);
                        reject(e);
                    });
                }
                else
                {
                    ExecuteOpened();
                    resolve();
                }
            });
        }

        public IPromise Close()
        {
            return new Promise((resolve, reject) =>
            {
                _isOpen = false;

                Closes();
                _events.onCloses.Invoke();

                OnBeforeClose();

                var promise = PrepareClose();
                if (promise != null)
                {
                    promise.Then(() =>
                    {
                        ExecuteClosed();
                        resolve();
                    }).Catch(e =>
                    {
                        Debug.LogError(e);
                        reject(e);
                    });
                }
                else
                {
                    ExecuteClosed();
                    resolve();
                }
            });
        }

        #endregion

        #region Virtual Members

        /// <summary>
        /// Called by View::Open(). Return a promise to delay the final opened call.
        /// </summary>
        protected virtual IPromise PrepareOpen() => null;

        /// <summary>
        /// Called by View::Close(). Return a promise to delay the final closed call.
        /// </summary>
        protected virtual IPromise PrepareClose() => null;

        /// <summary>
        /// Called every time the view starts opening, can be overloaded
        /// </summary>
        protected virtual void OnBeforeOpen()
        {
        }

        /// <summary>
        /// Called every time the view starts closing, can be overloaded
        /// </summary>
        protected virtual void OnBeforeClose()
        {
        }

        /// <summary>
        /// Called every time the view finished opening, can be overloaded
        /// </summary>
        protected virtual void OnOpen()
        {
        }

        /// <summary>
        /// Called every time the menu finished closing, can be overloaded
        /// </summary>
        protected virtual void OnClose()
        {
        }

        #endregion

        #region State Control

        /// <summary>
        /// Called when all contents report being ready through their returned promise object. Calls MenuContent::Open()
        /// on al contents.
        /// </summary>
        private void ExecuteOpened()
        {
            Opened();
            _events.onOpened.Invoke();
            OnOpen();
        }

        private void ExecuteClosed()
        {
            gameObject.SetActive(false);
            Closed();
            _events.onClosed.Invoke();
            OnClose();
        }

        #endregion

        [Serializable]
        private class UnityEvents
        {
            [SerializeField] private UnityEvent _onOpens = null;
            [SerializeField] private UnityEvent _onCloses = null;
            [SerializeField] private UnityEvent _onOpened = null;
            [SerializeField] private UnityEvent _onClosed = null;

            public UnityEvent onOpens => _onOpens;
            public UnityEvent onCloses => _onCloses;
            public UnityEvent onOpened => _onOpened;
            public UnityEvent onClosed => _onClosed;
        }
    }
}