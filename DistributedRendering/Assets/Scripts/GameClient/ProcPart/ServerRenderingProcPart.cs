namespace GameClient
{

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;
    private readonly ICameraViewController _cameraViewController;
    private readonly INamedPipeClient _namedPipeClient;

    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingUIController = renderingUIController;
        _cameraViewController = cameraViewController;
        _namedPipeClient = namedPipeClient;
    }

    public void Activate()
    {
        _renderingUIController.Activate();

        _cameraViewController.OnUpdateTransform += (transform) =>
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
    }

    public void Deactivate()
    {
        _renderingUIController.Deactivate();
    }

    public void RenderImageBuffer(byte[] buffer)
    {
        _renderingUIController.RenderImageBuffer(buffer);
    }
}

}
