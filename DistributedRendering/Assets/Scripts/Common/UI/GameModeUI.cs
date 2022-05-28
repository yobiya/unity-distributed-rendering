using System;
using UnityEngine;

namespace Common
{

public class GameModeUI : MonoBehaviour, IGameModeUI
{
    [SerializeField]
    ButtonUIViewAdapter _gameClientModeButton;

    [SerializeField]
    ButtonUIViewAdapter _renderingServerModeButton;

    public event Action OnSelectedGameClient;
    public event Action OnSelectedRenderingServer;

    public void Activate()
    {
        _gameClientModeButton.OnClicked += SelectedGameClientAction;
        _renderingServerModeButton.OnClicked += SelectedRenderingserverAction;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _gameClientModeButton.OnClicked -= SelectedGameClientAction;
        _renderingServerModeButton.OnClicked -= SelectedRenderingserverAction;
        gameObject.SetActive(false);
    }

    private void SelectedGameClientAction() => OnSelectedGameClient?.Invoke();
    private void SelectedRenderingserverAction() => OnSelectedRenderingServer?.Invoke();
}

}
