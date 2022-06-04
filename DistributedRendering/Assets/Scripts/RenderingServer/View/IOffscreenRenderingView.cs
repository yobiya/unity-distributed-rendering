using UnityEngine;

namespace RenderingServer
{

public interface IOffscreenRenderingView
{
    RenderTexture RenderTexture { get; }

    void Activate();
    void Deactivate();
}

}
