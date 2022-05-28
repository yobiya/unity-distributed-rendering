using Common;

namespace RenderingServer
{

public class DebugRenderingProcPart : IDebugRenderingProcPart
{
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;

    public DebugRenderingProcPart(ServiceLocator sl)
    {
        _debugRenderingUIControler = sl.Get<IDebugRenderingUIControler>();
    }

    public void Activate(IRenderTextureView textureView)
    {
        _debugRenderingUIControler.Activate(textureView);
    }

    public void Deactivate()
    {
        _debugRenderingUIControler.Deactivate();
    }
}

}
