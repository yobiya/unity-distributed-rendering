using UnityEngine;
using VContainer;

namespace RenderingServer
{

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    private readonly IOffscreenRenderingView _offscreenRenderingView;

    public RenderTexture RenderTexture => _offscreenRenderingView.RenderTexture;

    [Inject]
    public OffscreenRenderingViewController(IOffscreenRenderingView offscreenRenderingView)
    {
        _offscreenRenderingView = offscreenRenderingView;
    }

    public void Activate()
    {
        _offscreenRenderingView.Activate();
    }

    public void Deactivate()
    {
        _offscreenRenderingView.Deactivate();
    }
}

}
