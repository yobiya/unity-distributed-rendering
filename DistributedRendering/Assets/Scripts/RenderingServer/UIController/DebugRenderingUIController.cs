using Common;

namespace RenderingServer
{

public interface IDebugRenderingUIControler
{
    void Activate();
    void Deactivate();
}

public class DebugRenderingUIControler : IDebugRenderingUIControler
{
    private IDebugRenderingUI _debugRenderingUI;

    public DebugRenderingUIControler(ServiceLocator sl)
    {
        _debugRenderingUI = sl.Get<IDebugRenderingUI>();
    }

    public void Activate()
    {
        _debugRenderingUI.Activate();
    }

    public void Deactivate()
    {
        _debugRenderingUI.Deactivate();
    }
}

}
