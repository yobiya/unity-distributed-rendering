using MessagePackFormat;
using UnityEngine;
using VContainer;

namespace GameClient
{

public interface IRenderingUIController
{
    void Activate(Camera camera, SetupData setupData);
    void Deactivate();
    void RenderBaseImage();
    void MargeImage(byte[] buffer);
}

public class RenderingUIController : IRenderingUIController
{
    private readonly RenderingUI _renderingUI;

    [Inject]
    public RenderingUIController(RenderingUI renderingUI)
    {
        _renderingUI = renderingUI;
    }

    public void Activate(Camera camera, SetupData setupData)
    {
        _renderingUI.Activate(camera, setupData);
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
