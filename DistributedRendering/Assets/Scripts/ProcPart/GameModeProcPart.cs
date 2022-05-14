public class GameModeProcPart
{
    public GameModeProcPart(
        IGameModeUIViewController gameModeUIViewController,
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        IGameClientConnectingWaitUIViewController gameClientConnectingWaitUIViewController)
    {
        gameModeUIViewController.OnSelectedGameClientMode += () =>
        {
            renderingServerConnectingUIViewController.IsActive = true;
            gameClientConnectingWaitUIViewController.IsActive = false;
        };
    }
}
