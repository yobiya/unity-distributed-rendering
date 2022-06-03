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
    private readonly ISyncronizeDataCreator _syncronizeDataCreator;
    private readonly InversionProc _inversionProc = new InversionProc();

    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient,
        ISyncronizeDataCreator syncronizeDataCreator)
    {
        _renderingUIController = renderingUIController;
        _cameraViewController = cameraViewController;
        _namedPipeClient = namedPipeClient;
        _syncronizeDataCreator = syncronizeDataCreator;
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(_renderingUIController.Activate, _renderingUIController.Deactivate);

        Action<Transform> updateCameraTransform = (transform) =>
        {
            string text
                = $"@camera:"
                + $"{transform.position.x},"
                + $"{transform.position.y},"
                + $"{transform.position.z},"
                + $"{transform.forward.x},"
                + $"{transform.forward.y},"
                + $"{transform.forward.z}";
            _namedPipeClient.Write(text);
        };
        _inversionProc.Register(
            () => _cameraViewController.OnUpdateTransform += updateCameraTransform,
            () => _cameraViewController.OnUpdateTransform -= updateCameraTransform);

        while (true)
        {
            var buffer = await _namedPipeClient.RecieveDataAsync();
            _renderingUIController.RenderImageBuffer(buffer);
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
