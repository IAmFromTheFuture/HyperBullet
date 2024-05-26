using UnityEngine;
using HB.Utilities;
using System.Collections.Generic;
using static UIDefines;

namespace HB.Core.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] Transform _activeCanvas;
        [SerializeField] Transform _inActiveCanvas;

        [SerializeField] PanelType _initialPanel;

        private bool _isInitialized = false;
        private UIPanel _currentPanel = null;

        protected bool IsInitialized => _isInitialized;
        public UIPanel CurrentPanel => _currentPanel;

        private Dictionary<PanelType, UIPanel> _allPanels;
        private Queue<UIPanel> _currentOperations;

        #region Singleton UIController
        private static UIController _instance = null;
        public static UIController Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            initialize();
        }
        #endregion

        private void initialize()
        {
            List<UIPanel> temp = new List<UIPanel>();
            _inActiveCanvas.GetComponentsInChildren(true, temp);

            if (temp.Count == 0)
            {
                LoggerUtility.LogError("Error Initializing Panels. Please make sure the panels exist inside the \"InActive Canvas\"");
                return;
            }

            _allPanels = new Dictionary<PanelType, UIPanel>();
            _currentOperations = new Queue<UIPanel>();

            for (int i = 0; i < temp.Count; i++)
            {
                if (_allPanels.ContainsKey(temp[i].Panel))
                {
                    LoggerUtility.LogError("Duplicate Key Found. Please make sure the assigned keys are unique in the Inspector");
                    return;
                }

                _allPanels.Add(temp[i].Panel, temp[i]);

                temp[i].gameObject.SetActive(false);
            }

            _isInitialized = true;

            LoggerUtility.Log("Found " + temp.Count + " Panels. Initialization Complete.");
            onInitializationComplete();
        }

        private void onInitializationComplete()
        {
            ChangePanel(_initialPanel, true);
        }

        public void ChangePanel(PanelType panelType, bool addToStack = false)
        {
            if (!tryGetPanel(panelType, out UIPanel panel)) return;

            if (_currentPanel != null
                && _currentPanel.Equals(panel))
            {
                return;
            }

            changePanel(panel, addToStack);
        }

        private bool tryGetPanel(PanelType panelType, out UIPanel panel)
        {
            if (!_isInitialized || !_allPanels.TryGetValue(panelType, out panel))
            {
                panel = null;
                return false;
            }

            return true;
        }

        private void changePanel(UIPanel panel, bool addToStack)
        {
            if (_currentPanel != null)
            {
                // hide current panel
                changePanelState(_currentPanel);
            }

            changePanelState(panel);
        }

        private void changePanelState(UIPanel panel)
        {
            int newState = Mathf.Abs((int)panel.PanelState - 1);

            panel.PanelState = (PanelState)newState;
            move(panel);
        }

        private void move(UIPanel panel)
        {
            if (_currentPanel != null
                && _currentPanel.IsBusyWithOperation)
            {
                _currentOperations.Enqueue(panel);
                return;
            }

            switch (panel.PanelState)
            {
                case PanelState.IN_ACTIVE:
                    panel.Hide();
                    panel.transform.SetParent(_inActiveCanvas);
                    break;
                case PanelState.ACTIVE:
                    panel.transform.SetParent(_activeCanvas);
                    panel.Show();
                    _currentPanel = panel;
                    break;
            }
        }

        public void CheckCurrentOperations()
        {
            if (_currentOperations.TryDequeue(out UIPanel panel))
            {
                move(panel);
            }
        }
    }
}
