namespace RenderingServer
{

public class OffscreenRenderingProcPart : IOffscreenRenderingProcPart
{
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;

    public OffscreenRenderingProcPart(IOffscreenRenderingViewController offscreenRenderingViewController)
    {
        _offscreenRenderingViewController = offscreenRenderingViewController;
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
