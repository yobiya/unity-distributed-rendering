using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace GameClient
{

public interface IServerRenderingProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;
    private readonly ISyncronizeSerializeViewController _syncronizeSerializeViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ISyncronizeSerializeViewController syncronizeSerializeViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingUIController = renderingUIController;
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
            var sendData = _syncronizeSerializeViewController.Serialize();
            await _namedPipeClient.SendDataAsync(sendData, cancellationTokenSource.Token);

            var recievedData = await _namedPipeClient.RecieveDataAsync(cancellationTokenSource.Token);
            _renderingUIController.RenderImageBuffer(recievedData);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
