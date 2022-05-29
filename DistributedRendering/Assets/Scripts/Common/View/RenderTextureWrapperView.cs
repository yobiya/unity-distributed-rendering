using UnityEngine;

namespace Common
{

public class RenderTextureWrapperView : IRenderTextureView
{
    private RenderTexture _renderTexture;

    public RenderTexture RenderTexture => _renderTexture;

    public void Activate()
    {
        _renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
    }

    public void Deactivate()
    {
        RenderTexture.Destroy(_renderTexture);
    }
}

}
