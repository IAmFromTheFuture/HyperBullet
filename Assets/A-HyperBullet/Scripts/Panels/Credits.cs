using UnityEngine;
using UnityEngine.UI;
using HB.Core.Controllers;

public class Credits : AnimatableUIPanel
{
    [SerializeField] private Button _backButton;

    #region UIPanel Methods
    protected override void Initialize()
    {
    }

    protected override void Bind()
    {
        _backButton.onClick.AddListener(onClickBack);
    }

    protected override void Unbind()
    {
        _backButton.onClick.RemoveListener(onClickBack);
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
        onClickBack();
    }
    #endregion

    private void onClickBack()
    {
        UIController.Instance.OnClickBack();
    }

}
