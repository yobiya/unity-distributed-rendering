public class ProcPartBinder
{
    public static void Bind(IGameModeProcPart gameModeProcPart, IRenderingServerConnectingProcPart renderingServerConnectingProcPart)
    {
        gameModeProcPart.OnSelectedGameClientMode += renderingServerConnectingProcPart.Activate;
    }
}
