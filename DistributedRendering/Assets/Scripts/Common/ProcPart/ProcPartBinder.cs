using RenderingServer;

namespace Common
{

public class ProcPartBinder
{
    public static void Bind(
        IGameModeProcPart gameModeProcPart,
        IRenderingServerConnectingProcPart renderingServerConnectingProcPart,
        IGameClientWaitConnectionProcPart gameClientWaitConnectionProcPart,
        IOffscreenRenderingProcPart offscreenRenderingProcPart)
    {
        gameModeProcPart.OnSelectedGameClientMode += renderingServerConnectingProcPart.Activate;
        gameModeProcPart.OnSelectedRenderingServerMode += () =>
        {
            gameClientWaitConnectionProcPart.Activate();
            gameClientWaitConnectionProcPart.StartWaitConnection();
        };

        gameClientWaitConnectionProcPart.OnConnected += offscreenRenderingProcPart.Activate;
    }
}

}
