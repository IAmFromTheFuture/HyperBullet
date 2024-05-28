using UnityEngine;
using HB.Utilities;
using System.Collections.Generic;
using static UIDefines;

namespace HB.Core.Controllers
{
    #region DATA MODELS
    public class PanelDataForQueue
    {
        public UIPanel Panel;
        public bool AddToStack;

        public PanelDataForQueue(UIPanel panel, bool addToStack)
        {
            Panel = panel;
            AddToStack = addToStack;
        }
    }
    #endregion

    public class UIController : MonoBehaviour
    {
        [SerializeField] PanelName _initialPanel;

        [SerializeField] Transform _activeCanvas;
        [SerializeField] Transform _inActiveCanvas;

        private bool _isInitialized = false;
        private UIPanel _currentPanel = null;

        protected bool IsInitialized => _isInitialized;
        public UIPanel CurrentPanel => _currentPanel;

        private Dictionary<PanelName, UIPanel> _allPanels;
        private Queue<PanelDataForQueue> _currentOperations;
        private Stack<UIPanel> _panelStack = new Stack<UIPanel>();

        private CanvasGroup _canvasGroup;

        private Color _logColor = Color.green;

        #region Singleton UIController
        private static UIController _instance = null;
        public static UIController Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
                initialize();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Operations
        private void initialize()
        {
            _allPanels = new Dictionary<PanelName, UIPanel>();
            _currentOperations = new Queue<PanelDataForQueue>();
            _panelStack = new Stack<UIPanel>();
            _canvasGroup = _activeCanvas.gameObject.AddComponent<CanvasGroup>();

            DisableInput();
            loadAllPanels();

            if (_allPanels.Count.Equals(0))
            {
                LoggerUtility.LogError("Error Initializing Panels. Please make sure the panels exist inside the 'InActive Canvas'.");
                return;
            }

            LoggerUtility.Log("Initialization Complete.");

            _isInitialized = true;
            Invoke(nameof(onInitializationComplete), 0.5f);
        }

        private void loadAllPanels()
        {
            List<UIPanel> panels = new List<UIPanel>();
            _inActiveCanvas.GetComponentsInChildren(true, panels);

            for (int i = 0; i < panels.Count; i++)
            {
                if (_allPanels.ContainsKey(panels[i].Panel))
                {
                    LoggerUtility.LogError("Duplicate Key Found. Please make sure the assigned keys are unique in the Inspector");
                    continue;
                }

                _allPanels.Add(panels[i].Panel, panels[i]);
                panels[i].gameObject.SetActive(false);
            }

            LoggerUtility.Log("Found " + panels.Count + " Panels");
        }

        private void onInitializationComplete()
        {
            ChangePanel(_initialPanel, true);
        }

        public void ChangePanel(PanelName panelType, bool addToStack = false)
        {
            if (!_allPanels.TryGetValue(panelType, out UIPanel panel)) return;

            if (_currentPanel != null && _currentPanel.Equals(panel))
            {
                return;
            }

            changePanel(panel, addToStack);
        }

        private void changePanel(UIPanel panel, bool addToStack)
        {
            DisableInput();

            if (_currentPanel != null)
            {
                // hide current panel
                changePanelState(_currentPanel, false);
            }

            changePanelState(panel, addToStack);
        }

        private void changePanelState(UIPanel panel, bool addToStack)
        {
            int newState = Mathf.Abs((int)panel.PanelState - 1);

            panel.PanelState = (PanelState)newState;
            move(panel, addToStack);
        }

        private void move(UIPanel panel, bool addToStack)
        {
            if (_currentPanel != null
                && _currentPanel.IsBusyWithOperation)
            {
                _currentOperations.Enqueue(new PanelDataForQueue(panel, addToStack));
                return;
            }

            switch (panel.PanelState)
            {
                case PanelState.IN_ACTIVE:
                    deactivatePanel(panel);
                    break;
                case PanelState.ACTIVE:
                    if (addToStack)
                        _panelStack.Push(panel);

                    activatePanel(panel);
                    break;
            }
        }

        private void deactivatePanel(UIPanel panel)
        {
            panel.Hide();
            panel.transform.SetParent(_inActiveCanvas);
        }

        private void activatePanel(UIPanel panel)
        {
            panel.transform.SetParent(_activeCanvas);
            panel.Show();
            _currentPanel = panel;
        }

        public void CheckCurrentOperations()
        {
            if (_currentOperations.Count > 0
                && _currentOperations.TryDequeue(out PanelDataForQueue panelData))
            {
                LoggerUtility.PrettyLog("ENQUEUING :: " + panelData.Panel.Panel, _logColor);

                move(panelData.Panel, panelData.AddToStack);
            }
            else
            {
                EnableInput();
            }
        }
        #endregion

        public void OnClickBack()
        {
            if (_panelStack.Count == 0) return;

            UIPanel topPanel = _panelStack.Peek();

            if (_panelStack.Count == 1 && topPanel.Panel != CurrentPanel.Panel)
            {
                ChangePanel(topPanel.Panel, false);
                return;
            }

            _panelStack.Pop();

            ChangePanel(_panelStack.Peek().Panel, false);

        }

        /// <summary>
        /// Use this to disable input on the Active Canvas. CAUTION: Must manually enable input again.
        /// </summary>
        public void DisableInput()
        {
            LoggerUtility.PrettyLog("Disabling CANVASGROUP", _logColor);

            _canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Use this to enable input on the Active Canvas.
        /// </summary>
        public void EnableInput()
        {
            LoggerUtility.PrettyLog("Enabling CANVASGROUP", _logColor);

            _canvasGroup.blocksRaycasts = true;
        }
    }
}
