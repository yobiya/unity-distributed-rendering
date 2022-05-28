using Common;

namespace RenderingServer
{

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    public interface IViewCollection
    {
        ICameraView Camera { get; }
    }

    private readonly IViewCollection _viewCollection;

    private RenderTextureWrapperView _textureView;

    public IRenderTextureView Texture { get; private set; }

    public OffscreenRenderingViewController(IViewCollection viewCollection)
    {
        _viewCollection = viewCollection;
    }

    public void Activate()
    {
        _textureView = new RenderTextureWrapperView();
        _textureView.Activate();

        Texture = _textureView;
    }

    public void Deactivate()
    {
        Texture = null;

        _textureView.Deactivate();
        _textureView = null;
    }
}

}
