using System.Threading;
using Common;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;
    private readonly ICameraViewController _cameraViewController;
    private readonly ISyncronizeSerializeViewController _syncronizeSerializeViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly InversionProc _inversionProc = new InversionProc();

    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ICameraViewController cameraViewController,
        ISyncronizeSerializeViewController syncronizeSerializeViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingUIController = renderingUIController;
        _cameraViewController = cameraViewController;
        _namedPipeClient = namedPipeClient;
        _syncronizeSerializeViewController = syncronizeSerializeViewController;
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(_renderingUIController.Activate, _renderingUIController.Deactivate);

        var cancellationTokenSource = new CancellationTokenSource();
        _inversionProc.Register(() => {}, cancellationTokenSource.Cancel);

        while (!cancellationTokenSource.IsCancellationRequested)
        {
            var sendText = _syncronizeSerializeViewController.Create();
            _namedPipeClient.Write(sendText);

            var recievedData = await _namedPipeClient.RecieveDataAsync(cancellationTokenSource.Token);
            _renderingUIController.RenderImageBuffer(recievedData);
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
