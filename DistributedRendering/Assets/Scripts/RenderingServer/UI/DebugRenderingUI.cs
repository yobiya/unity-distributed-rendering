using Common;

namespace RenderingServer
{

public interface IDebugRenderingUI
{
    void Activate(IRenderTextureView textureView);
    void Deactivate();
}

public class DebugRenderingUI : IDebugRenderingUI
{
    public interface IUICollection
    {
    }

    public void Activate(IRenderTextureView textureView)
    {
    }

    public void Deactivate()
    {
    }
}

}
