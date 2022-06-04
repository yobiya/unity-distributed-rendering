using UnityEngine;

namespace RenderingServer
{

public interface IDebugRenderingUI
{
    void Activate(RenderTexture renderTexture);
    void Deactivate();
}

}
