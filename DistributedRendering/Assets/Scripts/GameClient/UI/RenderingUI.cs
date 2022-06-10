using Common;
using MessagePackFormat;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{

public class RenderingUI : MonoBehaviour
{
    [SerializeField]
    private RawImage _selfRenderingImage;

    [SerializeField]
    private RawImage _serverRenderingImage;

    private readonly InversionProc _inversionProc = new InversionProc();
    private Camera _camera;
    private RenderTexture _selfRenderTexture;
    private Texture2D _serverRenderingTexture2d;

    public void Activate(Camera camera, SetupData setupData)
    {
        _camera = camera;
        _selfRenderingImage.rectTransform.sizeDelta = new Vector2(RenderingDefinisions.RenderingTextureWidth, RenderingDefinisions.RenderingTextureHight);
        _selfRenderingImage.rectTransform.anchoredPosition = Vector2.zero;
        _serverRenderingImage.rectTransform.sizeDelta = new Vector2(setupData.renderingRect.width, setupData.renderingRect.height);
        _serverRenderingImage.rectTransform.anchoredPosition = new Vector2(setupData.renderingRect.x, setupData.renderingRect.y);

        _inversionProc.Register(
            () => gameObject.SetActive(true),
            () => gameObject.SetActive(false));

        _inversionProc.Register(
            () => _selfRenderTexture
                = new RenderTexture(
                    RenderingDefinisions.RenderingTextureWidth,
                    RenderingDefinisions.RenderingTextureHight,
                    16,
                    RenderTextureFormat.ARGB32),
            () => RenderTexture.Destroy(_selfRenderTexture));

        _inversionProc.Register(
            () => _selfRenderingImage.texture = _selfRenderTexture,
            () => _selfRenderingImage.texture = null);

        _inversionProc.Register(
            () => _serverRenderingTexture2d
                    = new Texture2D(
                        setupData.renderingRect.width,
                        setupData.renderingRect.height,
                        TextureFormat.ARGB32,
                        false),
            () => Texture2D.Destroy(_serverRenderingTexture2d));

        _inversionProc.Register(
            () => _serverRenderingImage.texture = _serverRenderingTexture2d,
            () => _serverRenderingImage.texture = null);

        _inversionProc.Register(
            () => _camera.targetTexture = _selfRenderTexture,
            () => _camera.targetTexture = null);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public void RenderBaseImage()
    {
        _camera.Render();
    }

    public void MargeImage(byte[] buffer)
    {
        _serverRenderingTexture2d.LoadRawTextureData(buffer);
        _serverRenderingTexture2d.Apply();
    }
}

}
