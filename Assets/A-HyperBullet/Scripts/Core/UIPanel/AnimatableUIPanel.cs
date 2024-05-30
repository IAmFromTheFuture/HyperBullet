using Core.Utilities;
using System;
using UnityEngine;

namespace Core.Controllers
{
    public abstract class AnimatableUIPanel : UIPanel
    {
        [SerializeField] private bool _hasEntry = false;
        [SerializeField] private bool _hasExit = false;

        private Animator _anim;

        public bool HasEntry => _hasEntry;
        public bool HasExit => _hasExit;

        private Action _onAnimationCompleted;

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

            gameObject.SetActive(true);
            IsBusyWithOperation = true;

            OnPanelShowImmediate();

            _anim.SetTrigger("Show");
            _onAnimationCompleted = show;

            Invoke(nameof(waitForTransition), 0.2f);
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

            _anim.SetTrigger("Hide");
            _onAnimationCompleted = hide;

            Invoke(nameof(waitForTransition), 0.2f);
        }

        private void waitForTransition()
        {
            float timeToWait = 0.0f;
            AnimatorClipInfo[] _ = _anim.GetCurrentAnimatorClipInfo(0);

            if (_.Length > 0)
            {
                timeToWait = _[0].clip.length;
                LoggerUtility.Log("Time to wait == " + timeToWait);
            }

            MEC.Timing.CallDelayed(timeToWait, () =>
            {
                _onAnimationCompleted();
                _onAnimationCompleted = null;
            });
        }

        private void hide()
        {
            OnPanelHide();

            IsBusyWithOperation = false;
            gameObject.SetActive(false);

            UIController.Instance.HideDeactivatedPanel(this);
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
