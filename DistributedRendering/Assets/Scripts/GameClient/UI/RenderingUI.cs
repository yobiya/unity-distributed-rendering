using Common;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{

public interface IRenderingUI
{
    void Activate();
    void Deactivate();
    void SetImageBuffer(byte[] buffer);
}

public class RenderingUI : MonoBehaviour, IRenderingUI
{
    [SerializeField]
    private RawImage _image;

    private Texture2D _texture2d;
    private InversionProc _inversionProc = new InversionProc();

    public void Activate()
    {
        _inversionProc.Register(
            () => gameObject.SetActive(true),
            () => gameObject.SetActive(false));

        _inversionProc.Register(
            () => _texture2d
                    = new Texture2D(
                        RenderingDefinisions.RenderingTextureWidth,
                        RenderingDefinisions.RenderingTextureHight,
                        TextureFormat.ARGB32,
                        false),
            () => Texture2D.Destroy(_texture2d));

        _inversionProc.Register(
            () => _image.texture = _texture2d,
            () => _image.texture = null);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public void SetImageBuffer(byte[] buffer)
    {
        _texture2d.LoadRawTextureData(buffer);
        _texture2d.Apply();
    }
}

}
