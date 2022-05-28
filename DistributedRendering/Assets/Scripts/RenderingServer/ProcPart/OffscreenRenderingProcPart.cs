using System;
using Common;

namespace RenderingServer
{

public class OffscreenRenderingProcPart : IOffscreenRenderingProcPart
{
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;

    public event Action<IRenderTextureView> OnActivated;

    public OffscreenRenderingProcPart(ServiceLocator sl)
    {
        _offscreenRenderingViewController = sl.Get<IOffscreenRenderingViewController>();
    }

    public void Activate()
    {
        _offscreenRenderingViewController.Activate();

        OnActivated?.Invoke(null);
    }

    public void Deactivate()
    {
        _offscreenRenderingViewController.Deactivate();
    }
}

}
