using UnityEngine;
using static Defines.UI;

namespace Core.Controllers
{
    public abstract class UIPanel : MonoBehaviour
    {
        [SerializeField] private PanelName _panel;
        [SerializeField] private PanelState _panelState;

        private bool _isInitialized = false;

        public PanelName Panel => _panel;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                && !IsBusyWithOperation)
            {
                OnNativeBackClicked();
            }
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

            UIController.Instance.HideDeactivatedPanel(this);
            UIController.Instance.CheckCurrentOperations();
        }

        protected virtual void Initialize() { }
        protected virtual void Bind() { }
        protected virtual void Unbind() { }

        protected virtual void OnPanelShow() { }
        protected virtual void OnPanelHide() { }
        protected virtual void OnNativeBackClicked() { }
    }
}
