using UnityEngine;

namespace Common
{

public interface IRenderTextureView
{
    RenderTexture RenderTexture { get; }

    void Activate();
    void Deactivate();
}

}
