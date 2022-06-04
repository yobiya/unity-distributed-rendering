using System;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameClient
{

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;
    private readonly ICameraViewController _cameraViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ISyncronizeDataSerializer _syncronizeDataSerializer;
    private readonly InversionProc _inversionProc = new InversionProc();

    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient,
        ISyncronizeDataSerializer syncronizeDataSerializer)
    {
        _renderingUIController = renderingUIController;
        _cameraViewController = cameraViewController;
        _namedPipeClient = namedPipeClient;
        _syncronizeDataSerializer = syncronizeDataSerializer;
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(_renderingUIController.Activate, _renderingUIController.Deactivate);

        while (true)
        {
            var sendText = _syncronizeDataSerializer.Create();
            _namedPipeClient.Write(sendText);

            var recievedData = await _namedPipeClient.RecieveDataAsync();
            _renderingUIController.RenderImageBuffer(recievedData);
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
