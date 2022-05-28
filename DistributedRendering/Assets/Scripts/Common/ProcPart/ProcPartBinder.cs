using RenderingServer;

namespace Common
{

public class ProcPartBinder
{
    public static void Bind(
        ServiceLocator sl,
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

        gameClientWaitConnectionProcPart.OnConnected += () =>
        {
            sl.Get<IOffscreenRenderingProcPart>().Activate();
            sl.Get<IDebugRenderingProcPart>().Activate();
        };
    }
}

}
