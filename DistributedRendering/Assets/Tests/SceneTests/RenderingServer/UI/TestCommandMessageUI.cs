using System;
using UnityEngine;
using UnityEngine.UI;

namespace RenderingServer
{

public class TestCommandMessageUI : MonoBehaviour, ITestCommandMessageUI
{
    [SerializeField]
    private Button _renderButton;

    public event Action OnClickedRender;

    public void Activate()
    {
        gameObject.SetActive(true);
        _renderButton.onClick.AddListener(RenderButtonClicked);
    }

    public void Deactivate()
    {
        _renderButton.onClick.RemoveListener(RenderButtonClicked);
        gameObject.SetActive(false);
    }

    private void RenderButtonClicked() => OnClickedRender?.Invoke();
}

}
