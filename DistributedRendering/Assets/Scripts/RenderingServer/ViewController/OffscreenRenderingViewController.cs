using Cysharp.Threading.Tasks;
using MessagePackFormat;
using UnityEngine;
using VContainer;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    UniTask ActivateAsync(SetupData setupData);
    void Deactivate();
    byte[] Render();
}

public class OffscreenRenderingViewController : IOffscreenRenderingViewController
{
    private readonly IOffscreenRenderingView _offscreenRenderingView;
    private Texture2D _texture2d;
    private Rect _renderingRect;

    [Inject]
    public OffscreenRenderingViewController(IOffscreenRenderingView offscreenRenderingView)
    {
        _offscreenRenderingView = offscreenRenderingView;
    }

    public async UniTask ActivateAsync(SetupData setupData)
    {
        _renderingRect
            = new Rect(
                setupData.renderingRect.x,
                setupData.renderingRect.y,
                setupData.renderingRect.width,
                setupData.renderingRect.height);

        await _offscreenRenderingView.ActivateAsync();

        var renderTexture = _offscreenRenderingView.RenderTexture;
        _texture2d
            = new Texture2D(
                setupData.renderingRect.width,
                setupData.renderingRect.height,
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
        _texture2d.ReadPixels(_renderingRect, 0, 0);
        _texture2d.Apply();
        RenderTexture.active = null;

        return _texture2d.GetRawTextureData();
    }
}

}
