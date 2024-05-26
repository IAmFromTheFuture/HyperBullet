using UnityEngine;
using static UIDefines;

namespace HB.Core.Controllers
{
    public abstract class UIPanel : MonoBehaviour
    {

        [SerializeField] private PanelType _panel;
        [SerializeField] private PanelState _panelState;

        private bool _isInitialized = false;

        public PanelType Panel => _panel;

        public PanelState PanelState
        {
            get
            {
                return _panelState;
            }
            set
            {
                _panelState = value;
            }
        }

        protected bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IsBusyWithOperation { get; set; }

        protected virtual void Awake()
        {
            initialize();
        }

        private void OnEnable()
        {
            bind();
        }

        private void OnDisable()
        {
            unbind();
        }

        private void initialize()
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }
        }

        private void bind()
        {
            unbind();

            Bind();
        }

        private void unbind()
        {
            Unbind();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            OnPanelShow();

            UIController.Instance.CheckCurrentOperations();
        }

        public virtual void Hide()
        {
            OnPanelHide();

            gameObject.SetActive(false);

            UIController.Instance.CheckCurrentOperations();
        }

        protected virtual void Initialize() { }
        protected virtual void Bind() { }
        protected virtual void Unbind() { }

        protected virtual void OnPanelShow() { }
        protected virtual void OnPanelHide() { }
    }
}
