using Common;

namespace RenderingServer
{

public interface IDebugRenderingProcPart
{
    void Activate(IRenderTextureView textureView);
    void Deactivate();
}

}
