using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace RenderingServer
{

public class SyncronizeRenderingProcPart : ISyncronizeRenderingProcPart
{
    private readonly INamedPipeServer _namedPipeServer;
    private readonly ISyncronizeDeserializeViewController _syncronizeDeserializerViewController;
    private readonly ISyncCameraViewController _syncCameraViewController;
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public SyncronizeRenderingProcPart(
        INamedPipeServer namedPipeServer,
        ISyncronizeDeserializeViewController syncronizeDeserializerViewController,
        ISyncCameraViewController syncCameraViewController,
        IOffscreenRenderingViewController offscreenRenderingViewController,
        IDebugRenderingUIControler debugRenderingUIControler)
    {
        _namedPipeServer = namedPipeServer;
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
            // ゲームクライアントから同期するデータを受け取る
            var recievedData = await _namedPipeServer.RecieveDataAsync(cancellationTokenSource.Token);

            // 同期するデータをデシリアライズして、対応するオブジェクトに適用する
            _syncronizeDeserializerViewController.Deserialize(recievedData);

            _namedPipeServer.SendRenderingImage(_offscreenRenderingViewController.RenderTexture);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
