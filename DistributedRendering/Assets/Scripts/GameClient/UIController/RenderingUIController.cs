using MessagePackFormat;
using VContainer;

namespace GameClient
{

public interface IRenderingUIController
{
    void Activate(SetupData setupData);
    void Deactivate();
    void RenderBaseImage();
    void MargeImage(byte[] buffer);
}

public class RenderingUIController : IRenderingUIController
{
    private readonly IRenderingUI _renderingUI;

    [Inject]
    public RenderingUIController(IRenderingUI renderingUI)
    {
        _renderingUI = renderingUI;
    }

    public void Activate(SetupData setupData)
    {
        _renderingUI.Activate(setupData);
    }

    public void Deactivate()
    {
        _renderingUI.Deactivate();
    }

    public void RenderBaseImage()
    {
        _renderingUI.RenderBaseImage();
    }

    public void MargeImage(byte[] buffer)
    {
        _renderingUI.MargeImage(buffer);
    }
}

}
