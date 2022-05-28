using Common;

namespace RenderingServer
{

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    private readonly ServiceLocator _sl;
    private IOffscreenRenderingView _offscreenRenderingView;

    public IRenderTextureView RenderTexture => _offscreenRenderingView.RenderTexture;

    public OffscreenRenderingViewController(ServiceLocator sl)
    {
        _sl = sl;
    }

    public void Activate()
    {
        _offscreenRenderingView = _sl.Get<IOffscreenRenderingView>();
        _offscreenRenderingView.Activate();
    }

    public void Deactivate()
    {
        _offscreenRenderingView.Deactivate();
        _offscreenRenderingView = null;
    }
}

}
