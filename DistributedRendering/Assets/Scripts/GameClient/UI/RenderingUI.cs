using Common;
using MessagePackFormat;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{

public interface IRenderingUI
{
    void Activate(SetupData setupData);
    void Deactivate();
    void RenderBaseImage();
    void MargeImage(byte[] buffer);
}

public class RenderingUI : MonoBehaviour, IRenderingUI
{
    [SerializeField]
    private RawImage _serverRenderingImage;

    private Texture2D _serverRenderingTexture2d;
    private InversionProc _inversionProc = new InversionProc();

    public void Activate(SetupData setupData)
    {
        _serverRenderingImage.rectTransform.sizeDelta = new Vector2(setupData.renderingRect.width, setupData.renderingRect.height);

        _inversionProc.Register(
            () => gameObject.SetActive(true),
            () => gameObject.SetActive(false));

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
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public void RenderBaseImage()
    {
    }

    public void MargeImage(byte[] buffer)
    {
        _serverRenderingTexture2d.LoadRawTextureData(buffer);
        _serverRenderingTexture2d.Apply();
    }
}

}
