using Common;
using UnityEngine;
using UnityEngine.UI;

namespace RenderingServer
{

public class DebugRenderingUI : MonoBehaviour, IDebugRenderingUI
{
    [SerializeField]
    private RawImage _rawImage;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate(IRenderTextureView textureView)
    {
        gameObject.SetActive(true);

        _rawImage.texture = textureView.RenderTexture;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
