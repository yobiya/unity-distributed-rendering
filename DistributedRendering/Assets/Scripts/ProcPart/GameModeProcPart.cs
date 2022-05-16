using System;

public class GameModeProcPart
{
    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeProcPart(IGameModeUIViewController gameModeUIViewController)
    {
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
}
