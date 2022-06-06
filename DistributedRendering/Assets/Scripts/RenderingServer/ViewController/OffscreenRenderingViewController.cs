using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    RenderTexture RenderTexture { get; }

    UniTask ActivateAsync();
    void Deactivate();
}

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    private readonly IOffscreenRenderingView _offscreenRenderingView;

    public RenderTexture RenderTexture => _offscreenRenderingView.RenderTexture;

    [Inject]
    public OffscreenRenderingViewController(IOffscreenRenderingView offscreenRenderingView)
    {
        _offscreenRenderingView = offscreenRenderingView;
    }

    public async UniTask ActivateAsync()
    {
        await _offscreenRenderingView.ActivateAsync();
    }

    public void Deactivate()
    {
        _offscreenRenderingView.Deactivate();
    }
}

}
