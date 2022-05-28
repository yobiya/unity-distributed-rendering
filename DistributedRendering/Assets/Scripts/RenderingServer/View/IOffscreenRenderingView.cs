using Common;

namespace RenderingServer
{

public interface IOffscreenRenderingView
{
    IRenderTextureView RenderTexture { get; }

    void Activate();
    void Deactivate();
}

}
