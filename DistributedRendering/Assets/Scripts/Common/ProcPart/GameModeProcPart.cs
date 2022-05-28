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
        gameModeUIViewController.Activate();
        gameModeUIViewController.OnSelectedGameClientMode += () =>
        {
            gameModeUIViewController.Deactivate();
            OnSelectedGameClientMode?.Invoke();
        };
        gameModeUIViewController.OnSelectedRenderingServerMode += () =>
        {
            gameModeUIViewController.Deactivate();
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
