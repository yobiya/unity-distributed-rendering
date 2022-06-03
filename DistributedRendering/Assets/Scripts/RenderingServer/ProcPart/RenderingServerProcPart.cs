using System;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public class RenderingServerProcPart : IRenderingServerProcPart
{
    private readonly INamedPipeServer _namedPipeServer;
    private readonly ISyncCameraViewController _syncCameraViewController;
    private readonly InversionProc _inversionProc = new InversionProc();

    public RenderingServerProcPart(INamedPipeServer namedPipeServer, ISyncCameraViewController syncCameraViewController)
    {
        _namedPipeServer = namedPipeServer;
        _syncCameraViewController = syncCameraViewController;
    }

    public async UniTask Activate()
    {
        _inversionProc.Register(_syncCameraViewController.Activate, _syncCameraViewController.Deactivate);

         Action<string> syncEvent = (text) => _syncCameraViewController.Sync(text);
        _inversionProc.Register(
            () => _namedPipeServer.OnRecieved += syncEvent,
            () => _namedPipeServer.OnRecieved -= syncEvent);

        await _namedPipeServer.ReadCommandAsync();
    }

    public void Deactivate()
    {
        _namedPipeServer.Deactivate();
        _inversionProc.Inversion();
    }
}

}
