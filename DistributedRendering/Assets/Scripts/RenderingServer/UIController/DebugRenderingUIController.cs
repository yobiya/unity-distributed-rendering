using Common;
using UnityEngine;
using VContainer;

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
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public DebugRenderingUIControler(IDebugRenderingUI debugRenderingUI)
    {
        _debugRenderingUI = debugRenderingUI;
    }

    public void Activate(RenderTexture renderTexture)
    {
        _inversionProc.Register(
            () => _debugRenderingUI.Activate(renderTexture),
            _debugRenderingUI.Deactivate);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
