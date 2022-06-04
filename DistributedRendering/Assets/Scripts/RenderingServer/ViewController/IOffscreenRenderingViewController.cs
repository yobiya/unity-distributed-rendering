using UnityEngine;

namespace RenderingServer
{

public interface IOffscreenRenderingViewController
{
    RenderTexture RenderTexture { get; }

    void Activate();
    void Deactivate();
}

}
