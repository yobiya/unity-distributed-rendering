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

    public OffscreenRenderingViewController(IViewCollection viewCollection)
    {
        _viewCollection = viewCollection;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}

}
