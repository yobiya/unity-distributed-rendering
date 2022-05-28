using Common;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    IRenderTextureView RenderTexture { get; }

    void Activate();
    void Deactivate();
}

}
