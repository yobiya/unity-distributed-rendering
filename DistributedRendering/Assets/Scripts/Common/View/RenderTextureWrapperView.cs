using UnityEngine;

namespace Common
{

public class RenderTextureWrapperView : IRenderTextureView
{
    private RenderTexture _renderTexture;

    public RenderTexture Texture => _renderTexture;

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}

}
