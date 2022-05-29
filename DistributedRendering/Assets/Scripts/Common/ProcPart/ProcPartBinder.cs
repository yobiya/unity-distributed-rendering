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
        var offscreenRenderingProcPart = sl.Get<IOffscreenRenderingProcPart>();
        var responseRenderingProcPart = sl.Get<IResponseRenderingProcPart>();

        gameModeProcPart.OnSelectedGameClientMode += () =>
        {
            gameModeProcPart.Deactivate();
            renderingServerConnectingProcPart.Activate();
        };
        gameModeProcPart.OnSelectedRenderingServerMode += () =>
        {
            gameModeProcPart.Deactivate();
            gameClientWaitConnectionProcPart.Activate();
            gameClientWaitConnectionProcPart.StartWaitConnection();
        };

        gameClientWaitConnectionProcPart.OnConnected += () =>
        {
            offscreenRenderingProcPart.Activate();
            responseRenderingProcPart.Activate();
        };
        offscreenRenderingProcPart.OnActivated += sl.Get<IDebugRenderingProcPart>().Activate;
    }
}

}
