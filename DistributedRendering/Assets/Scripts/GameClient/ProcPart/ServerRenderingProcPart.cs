namespace GameClient
{

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;

    public ServerRenderingProcPart(IRenderingUIController renderingUIController)
    {
        _renderingUIController = renderingUIController;
    }

    public void Activate()
    {
        _renderingUIController.Activate();
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
