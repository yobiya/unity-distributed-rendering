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
    private readonly ISyncronizeDeserializeViewController _syncronizeDeserializerViewController;
    private readonly ISyncCameraViewController _syncCameraViewController;
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;
    private readonly InversionProc _inversionProc = new InversionProc();

    public SyncronizeRenderingProcPart(
        INamedPipeServer namedPipeServer,
        IResponseDataNamedPipe responseDataNamedPipe,
        ISyncronizeDeserializeViewController syncronizeDeserializerViewController,
        ISyncCameraViewController syncCameraViewController,
        IOffscreenRenderingViewController offscreenRenderingViewController,
        IDebugRenderingUIControler debugRenderingUIControler)
    {
        _namedPipeServer = namedPipeServer;
        _responseDataNamedPipe = responseDataNamedPipe;
        _syncronizeDeserializerViewController = syncronizeDeserializerViewController;
        _syncCameraViewController = syncCameraViewController;
        _offscreenRenderingViewController = offscreenRenderingViewController;
        _debugRenderingUIControler = debugRenderingUIControler;
    }

    public async UniTask ActivateAsync()
    {
        // _namedPipeServerは既に有効化されているので、Deactivateのみ登録する
        _inversionProc.RegisterInversion(_namedPipeServer.Deactivate);
        _inversionProc.Register(_syncCameraViewController.Activate, _syncCameraViewController.Deactivate);
        _inversionProc.Register(_offscreenRenderingViewController.Activate, _offscreenRenderingViewController.Deactivate);
        _inversionProc.Register(
            () => _debugRenderingUIControler.Activate(_offscreenRenderingViewController.RenderTexture),
            _debugRenderingUIControler.Deactivate);

        var cancellationTokenSource = new CancellationTokenSource();
        _inversionProc.RegisterInversion(cancellationTokenSource.Cancel);
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            var text = await _namedPipeServer.RecieveMessageAsync();
            if (String.Empty == text)
            {
                continue;
            }
            _syncCameraViewController.Sync(text);

            var recievedData = await _responseDataNamedPipe.RecieveDataAsync();
            _syncronizeDeserializerViewController.Deserialize(recievedData);

            _responseDataNamedPipe.SendRenderingImage(_offscreenRenderingViewController.RenderTexture);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
