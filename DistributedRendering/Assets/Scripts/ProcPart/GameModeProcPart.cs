public class GameModeProcPart
{
    public GameModeProcPart(
        IGameModeUIViewController gameModeUIViewController,
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        IGameClientConnectingWaitUIViewController gameClientConnectingWaitUIViewController)
    {
        gameModeUIViewController.IsActive = true;

        gameModeUIViewController.OnSelectedGameClientMode += () =>
        {
            gameModeUIViewController.IsActive = false;
            renderingServerConnectingUIViewController.IsActive = true;
            gameClientConnectingWaitUIViewController.IsActive = false;
        };

        gameModeUIViewController.OnSelectedRenderingServerMode += () =>
        {
            gameModeUIViewController.IsActive = false;
            renderingServerConnectingUIViewController.IsActive = false;
            gameClientConnectingWaitUIViewController.IsActive = true;
        };
    }
}
