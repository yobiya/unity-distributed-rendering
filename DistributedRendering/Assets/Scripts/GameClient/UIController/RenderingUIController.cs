using Common;

namespace GameClient
{

public class RenderingUIController : IRenderingUIController
{
    private readonly ServiceLocator _sl;

    public RenderingUIController(ServiceLocator sl)
    {
        _sl = sl;
    }

    public void Activate()
    {
        var ui = _sl.Get<IRenderingUI>();
        ui.Activate();
    }

    public void Deactivate()
    {
        var ui = _sl.Get<IRenderingUI>();
        ui.Deactivate();
    }

    public void RenderImageBuffer(byte[] buffer)
    {
        var ui = _sl.Get<IRenderingUI>();
        ui.SetImageBuffer(buffer);
    }
}

}
