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
    private readonly ServiceLocator _sl;
    private readonly IDebugRenderingUI _debugRenderingUI;

    public DebugRenderingUIControler(ServiceLocator sl)
    {
        _sl = sl;
        _debugRenderingUI = sl.Get<IDebugRenderingUI>();
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