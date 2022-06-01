using Common;

namespace RenderingServer
{

public interface IDebugRenderingUIControler
{
    void Activate(IRenderTextureView textureView);
    void Deactivate();
}

public class DebugRenderingUIControler : IDebugRenderingUIControler
{
    private readonly IDebugRenderingUI _debugRenderingUI;

    public DebugRenderingUIControler(IDebugRenderingUI debugRenderingUI)
    {
        _debugRenderingUI = debugRenderingUI;
    }

    public void Activate(IRenderTextureView textureView)
    {
        _debugRenderingUI.Activate(textureView);
    }

    public void Deactivate()
    {
        _debugRenderingUI.Deactivate();
    }
}

}
