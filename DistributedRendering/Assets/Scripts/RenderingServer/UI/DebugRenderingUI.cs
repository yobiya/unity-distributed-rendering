using UnityEngine;
using UnityEngine.UI;

namespace RenderingServer
{

public interface IDebugRenderingUI
{
    void Activate(RenderTexture renderTexture);
    void Deactivate();
}

public class DebugRenderingUI : MonoBehaviour, IDebugRenderingUI
{
    [SerializeField]
    private RawImage _rawImage;

    public void Activate(RenderTexture renderTexture)
    {
        gameObject.SetActive(true);

        _rawImage.texture = renderTexture;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
