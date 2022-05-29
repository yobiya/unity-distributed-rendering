using Common;

namespace GameClient
{

public class RenderingProcPart : IRenderingProcPart
{
    private readonly ServiceLocator _sl;

    public RenderingProcPart(ServiceLocator sl)
    {
        _sl = sl;
    }

    public void Activate()
    {
        _sl.Get<IRenderingUIController>().Activate();
    }

    public void Deactivate()
    {
        _sl.Get<IRenderingUIController>().Deactivate();
    }

    public void RenderImageBuffer(byte[] buffer)
    {
        _sl.Get<IRenderingUIController>().RenderImageBuffer(buffer);
    }
}

}
