using UnityEngine;

namespace Common
{

public class RenderTextureWrapperView : IRenderTextureView
{
    private readonly RenderTexture _renderTexture;

    public bool IsActive { get; set; }

    public RenderTextureWrapperView()
    {
    }
}

}
