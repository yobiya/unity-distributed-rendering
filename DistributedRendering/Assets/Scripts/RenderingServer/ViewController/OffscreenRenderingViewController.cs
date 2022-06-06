using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    UniTask ActivateAsync();
    void Deactivate();
    byte[] Render();
}

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    private readonly IOffscreenRenderingView _offscreenRenderingView;
    private Texture2D _texture2d;

    [Inject]
    public OffscreenRenderingViewController(IOffscreenRenderingView offscreenRenderingView)
    {
        _offscreenRenderingView = offscreenRenderingView;
    }

    public async UniTask ActivateAsync()
    {
        await _offscreenRenderingView.ActivateAsync();

        var renderTexture = _offscreenRenderingView.RenderTexture;
        _texture2d
            = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.ARGB32,
                false);
    }

    public void Deactivate()
    {
        Texture2D.Destroy(_texture2d);
        _offscreenRenderingView.Deactivate();
    }

    public byte[] Render()
    {
        _offscreenRenderingView.Render();

        var renderTexture = _offscreenRenderingView.RenderTexture;
        RenderTexture.active = renderTexture;
        _texture2d.ReadPixels(new Rect(renderTexture.width / 2, 0, renderTexture.width / 2, renderTexture.height), 0, 0);
        RenderTexture.active = null;
                                   
        return _texture2d.GetRawTextureData();
    }
}

}
