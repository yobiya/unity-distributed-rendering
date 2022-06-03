namespace GameClient
{

public class RenderingUIController : IRenderingUIController
{
    private readonly IRenderingUI _renderingUI;

    public RenderingUIController(IRenderingUI renderingUI)
    {
        _renderingUI = renderingUI;
    }

    public void Activate()
    {
        _renderingUI.Activate();
    }

    public void Deactivate()
    {
        _renderingUI.Deactivate();
    }

    public void RenderImageBuffer(byte[] buffer)
    {
        _renderingUI.SetImageBuffer(buffer);
    }
}

}
