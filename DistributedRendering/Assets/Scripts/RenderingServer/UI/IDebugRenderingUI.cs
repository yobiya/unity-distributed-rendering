using Common;

namespace RenderingServer
{

public interface IDebugRenderingUI
{
    void Activate(IRenderTextureView textureView);
    void Deactivate();
}

}
