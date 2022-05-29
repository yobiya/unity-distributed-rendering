using System;
using Common;

namespace RenderingServer
{

public class TestCommandMessageUIController : ITestCommandMessageUIController
{
    private readonly ServiceLocator _sl;

    public event Action OnRenderResuest;

    public TestCommandMessageUIController(ServiceLocator sl)
    {
        _sl = sl;
    }

    public void Activate()
    {
        var testCommandMessageUI = _sl.Get<ITestCommandMessageUI>();
        testCommandMessageUI.Activate();
        testCommandMessageUI.OnClickedRender += RenderResuest;
    }

    public void Deactivate()
    {
        var testCommandMessageUI = _sl.Get<ITestCommandMessageUI>();
        testCommandMessageUI.OnClickedRender -= RenderResuest;
        testCommandMessageUI.Deactivate();
    }

    private void RenderResuest() => OnRenderResuest?.Invoke();
}

}
