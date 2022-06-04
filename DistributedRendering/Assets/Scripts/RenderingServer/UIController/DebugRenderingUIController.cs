using UnityEngine;

namespace RenderingServer
{

public interface IDebugRenderingUIControler
{
    void Activate(RenderTexture renderTexture);
    void Deactivate();
}

public class DebugRenderingUIControler : IDebugRenderingUIControler
{
    private readonly IDebugRenderingUI _debugRenderingUI;

    public DebugRenderingUIControler(IDebugRenderingUI debugRenderingUI)
    {
        _debugRenderingUI = debugRenderingUI;
    }

    public void Activate(RenderTexture renderTexture)
    {
        _debugRenderingUI.Activate(renderTexture);
    }

    public void Deactivate()
    {
        _debugRenderingUI.Deactivate();
    }
}

}
