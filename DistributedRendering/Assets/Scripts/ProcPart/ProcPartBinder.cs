public class ProcPartBinder
{
    public static void Bind(
        IGameModeProcPart gameModeProcPart,
        IRenderingServerConnectingProcPart renderingServerConnectingProcPart,
        IGameClientWaitConnectionProcPart gameClientWaitConnectionProcPart)
    {
        gameModeProcPart.OnSelectedGameClientMode += renderingServerConnectingProcPart.Activate;
        gameModeProcPart.OnSelectedRenderingServerMode += () =>
        {
            gameClientWaitConnectionProcPart.Activate();
            gameClientWaitConnectionProcPart.StartWaitConnection();
        };
    }
}
