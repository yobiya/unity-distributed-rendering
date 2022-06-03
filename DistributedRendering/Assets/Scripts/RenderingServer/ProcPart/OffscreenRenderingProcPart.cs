using System;
using Common;

namespace RenderingServer
{

public class OffscreenRenderingProcPart : IOffscreenRenderingProcPart
{
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;

    public event Action<IRenderTextureView> OnActivated;

    public OffscreenRenderingProcPart(IOffscreenRenderingViewController offscreenRenderingViewController)
    {
        _offscreenRenderingViewController = offscreenRenderingViewController;
    }

    public void Activate()
    {
        _offscreenRenderingViewController.Activate();

        OnActivated?.Invoke(_offscreenRenderingViewController.RenderTexture);
    }

    public void Deactivate()
    {
        _offscreenRenderingViewController.Deactivate();
    }
}

}
