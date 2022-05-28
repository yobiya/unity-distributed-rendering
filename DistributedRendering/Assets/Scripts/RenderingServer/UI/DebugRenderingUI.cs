using Common;
using UnityEngine;
using UnityEngine.UI;

namespace RenderingServer
{

public class DebugRenderingUI : MonoBehaviour, IDebugRenderingUI
{
    [SerializeField]
    private Image _image;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate(IRenderTextureView textureView)
    {
        gameObject.SetActive(true);

        var material = new Material(_image.defaultMaterial);

        material.mainTexture = textureView.RenderTexture;

        _image.material.mainTexture = textureView.RenderTexture;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
