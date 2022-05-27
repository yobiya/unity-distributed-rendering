using Common;

namespace RenderingServer
{

public class OffscreenRenderingProcPart : IOffscreenRenderingProcPart
{
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;

    public OffscreenRenderingProcPart(ServiceLocator sl)
    {
        _offscreenRenderingViewController = sl.Get<IOffscreenRenderingViewController>();
    }

    public void Activate()
    {
        _offscreenRenderingViewController.Activate();
    }

    public void Deactivate()
    {
        _offscreenRenderingViewController.Deactivate();
    }
}

}
