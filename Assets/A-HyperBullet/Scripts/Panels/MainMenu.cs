using UnityEngine;
using UnityEngine.UI;
using HB.Core.Controllers;
using static UIDefines;

public class MainMenu : AnimatableUIPanel
{
    [SerializeField] private Button _credits;

    #region UIPanel Methods
    protected override void Initialize()
    {
    }

    protected override void Bind()
    {
        _credits.onClick.AddListener(onClickCredits);
    }

    protected override void Unbind()
    {
        _credits.onClick.RemoveListener(onClickCredits);
    }

    protected override void OnPanelShowImmediate()
    {
    }

    protected override void OnPanelShow()
    {
    }

    protected override void OnPanelHideImmediate()
    {
    }

    protected override void OnPanelHide()
    {
    }

    protected override void OnNativeBackClicked()
    {
    }
    #endregion

    private void onClickCredits()
    {
        UIController.Instance.ChangePanel(PanelName.CREDITS, true);
    }

}
