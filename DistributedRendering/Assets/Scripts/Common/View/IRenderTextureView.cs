using UnityEngine;

namespace Common
{

public interface IRenderTextureView : IBaseView
{
    RenderTexture Texture { get; }
}

}
