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

    public void Activate()
    {
        gameObject.SetActive(true);

        _texture2d = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        _image.texture = _texture2d;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetImageBuffer(byte[] buffer)
    {
        _texture2d.LoadRawTextureData(buffer);
        _texture2d.Apply();
    }
}

}
