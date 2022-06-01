using Common;

namespace RenderingServer
{

public class DebugRenderingProcPart : IDebugRenderingProcPart
{
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;

    public DebugRenderingProcPart(IDebugRenderingUIControler debugRenderingUIControler)
    {
        _debugRenderingUIControler = debugRenderingUIControler;
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
