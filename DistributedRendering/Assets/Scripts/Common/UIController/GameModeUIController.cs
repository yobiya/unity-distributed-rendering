using System;

namespace Common
{

public class GameModeUIController : IGameModeUIController
{
    private IGameModeUI _gameModeUI;

    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeUIController(ServiceLocator sl)
    {
        _gameModeUI = sl.Get<IGameModeUI>();

        _gameModeUI.OnSelectedGameClient += () => OnSelectedGameClientMode?.Invoke();
        _gameModeUI.OnSelectedRenderingServer += () => OnSelectedRenderingServerMode?.Invoke();
    }

    public void Activate()
    {
        _gameModeUI.Activate();
    }

    public void Deactivate()
    {
        _gameModeUI.Deactivate();
    }
}

}
