using Common;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    IRenderTextureView Texture { get; }

    void Activate();
    void Deactivate();
}

}
