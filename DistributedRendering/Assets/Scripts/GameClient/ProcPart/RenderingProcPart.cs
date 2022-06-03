namespace GameClient
{

public class RenderingProcPart : IRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;

    public RenderingProcPart(IRenderingUIController renderingUIController)
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
