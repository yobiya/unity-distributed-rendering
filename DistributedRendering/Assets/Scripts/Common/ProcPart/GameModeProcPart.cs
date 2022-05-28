using System;

namespace Common
{

public class GameModeProcPart : IGameModeProcPart
{
    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeProcPart(ServiceLocator sl)
    {
        var gameModeUIViewController = sl.Get<IGameModeUIController>();
        gameModeUIViewController.IsActive = true;
        gameModeUIViewController.OnSelectedGameClientMode += () =>
        {
            gameModeUIViewController.IsActive = false;
            OnSelectedGameClientMode?.Invoke();
        };
        gameModeUIViewController.OnSelectedRenderingServerMode += () =>
        {
            gameModeUIViewController.IsActive = false;
            OnSelectedRenderingServerMode?.Invoke();
        };
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}

}
