using UnityEngine;

namespace HB.Core.Controllers
{
    public abstract class AnimatableUIPanel : UIPanel
    {
        [SerializeField] private bool _hasEntry = false;
        [SerializeField] private bool _hasExit = false;

        private Animator _anim;

        public bool HasEntry => _hasEntry;
        public bool HasExit => _hasExit;

        protected override void Awake()
        {
            base.Awake();

            _anim = GetComponent<Animator>();
        }

        public override void Show()
        {
            if (!_hasEntry)
            {
                gameObject.SetActive(true);

                OnPanelShowImmediate();
                show();
                return;
            }

            IsBusyWithOperation = true;

            gameObject.SetActive(true);
            OnPanelShowImmediate();

            float time = 0.25f; // RSHARMA get animation length from clip for time
            Invoke(nameof(show), time);
        }

        private void show()
        {
            IsBusyWithOperation = false;

            OnPanelShow();

            UIController.Instance.CheckCurrentOperations();
        }

        public override void Hide()
        {
            if (!_hasExit)
            {
                OnPanelHideImmediate();
                hide();
                return;
            }

            IsBusyWithOperation = true;

            OnPanelHideImmediate();

            float time = 0.25f; // RSHARMA get animation length from clip for time
            Invoke("hide", time);
        }

        private void hide()
        {
            OnPanelHide();

            IsBusyWithOperation = false;
            gameObject.SetActive(false);

            UIController.Instance.CheckCurrentOperations();
        }

        /// <summary>
        /// Method that gets called immediately after Show but before the animation starts.
        /// </summary>
        protected virtual void OnPanelShowImmediate() { }

        /// <summary>
        /// Method that gets called immediately after Hide but before the animation starts.
        /// </summary>    
        protected virtual void OnPanelHideImmediate() { }
    }
}
