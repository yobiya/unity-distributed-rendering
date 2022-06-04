using System;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public class SyncronizeRenderingProcPart : ISyncronizeRenderingProcPart
{
    private readonly INamedPipeServer _namedPipeServer;
    private readonly IResponseDataNamedPipe _responseDataNamedPipe;
    private readonly ISyncCameraViewController _syncCameraViewController;
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;
    private readonly InversionProc _inversionProc = new InversionProc();

    public SyncronizeRenderingProcPart(
        INamedPipeServer namedPipeServer,
        IResponseDataNamedPipe responseDataNamedPipe,
        ISyncCameraViewController syncCameraViewController,
        IOffscreenRenderingViewController offscreenRenderingViewController,
        IDebugRenderingUIControler debugRenderingUIControler)
    {
        _namedPipeServer = namedPipeServer;
        _responseDataNamedPipe = responseDataNamedPipe;
        _syncCameraViewController = syncCameraViewController;
        _offscreenRenderingViewController = offscreenRenderingViewController;
        _debugRenderingUIControler = debugRenderingUIControler;
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(_syncCameraViewController.Activate, _syncCameraViewController.Deactivate);
        _inversionProc.Register(_offscreenRenderingViewController.Activate, _offscreenRenderingViewController.Deactivate);
        _inversionProc.Register(
            () => _debugRenderingUIControler.Activate(_offscreenRenderingViewController.RenderTexture),
            _debugRenderingUIControler.Deactivate);

        var cancellationTokenSource = new CancellationTokenSource();
        _inversionProc.Register(() => {}, cancellationTokenSource.Cancel);
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            var text = await _namedPipeServer.RecieveMessageAsync();
            if (String.Empty == text)
            {
                continue;
            }
            _syncCameraViewController.Sync(text);
            _responseDataNamedPipe.SendRenderingImage(_offscreenRenderingViewController.RenderTexture);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _namedPipeServer.Deactivate();
        _inversionProc.Inversion();
    }
}

}
