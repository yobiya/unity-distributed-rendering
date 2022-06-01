using System;

namespace Common
{

public class GameModeProcPart : IGameModeProcPart
{
    public event Action OnSelectedGameClientMode;
    public event Action OnSelectedRenderingServerMode;

    public GameModeProcPart(IGameModeUIController gameModeUIController)
    {
        gameModeUIController.Activate();
        gameModeUIController.OnSelectedGameClientMode += () =>
        {
            gameModeUIController.Deactivate();
            OnSelectedGameClientMode?.Invoke();
        };
        gameModeUIController.OnSelectedRenderingServerMode += () =>
        {
            gameModeUIController.Deactivate();
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
