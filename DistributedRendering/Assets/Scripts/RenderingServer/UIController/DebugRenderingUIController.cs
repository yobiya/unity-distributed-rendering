using Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace RenderingServer
{

public interface IDebugRenderingUIControler
{
    UniTask ActivateAsync();
    void Deactivate();
}

public class DebugRenderingUIControler : IDebugRenderingUIControler
{
    private readonly IDebugRenderingUI _debugRenderingUI;
    private readonly IOffscreenRenderingRefView _offscreenRenderingRefView;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public DebugRenderingUIControler(IDebugRenderingUI debugRenderingUI, IOffscreenRenderingRefView offscreenRenderingView)
    {
        _debugRenderingUI = debugRenderingUI;
        _offscreenRenderingRefView = offscreenRenderingView;
    }

    public async UniTask ActivateAsync()
    {
        await _offscreenRenderingRefView.WaitOnActivatedAsync();
        _inversionProc.Register(
            () => _debugRenderingUI.Activate(_offscreenRenderingRefView.RenderTexture),
            _debugRenderingUI.Deactivate);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
