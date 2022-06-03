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
    private readonly InversionProc _inversionProc = new InversionProc();

    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingUIController = renderingUIController;
        _cameraViewController = cameraViewController;
        _namedPipeClient = namedPipeClient;
    }

    public async UniTask Activate()
    {
        _inversionProc.Register(_renderingUIController.Activate, _renderingUIController.Deactivate);
        _inversionProc.Register(
            () => _namedPipeClient.OnRecieved += _renderingUIController.RenderImageBuffer,
            () => _namedPipeClient.OnRecieved -= _renderingUIController.RenderImageBuffer);

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

        await _namedPipeClient.StartConnectBinaryPipe();
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
