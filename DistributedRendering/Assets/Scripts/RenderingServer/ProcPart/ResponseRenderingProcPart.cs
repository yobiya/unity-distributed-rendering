using Common;

namespace RenderingServer
{

public class ResponseRenderingProcPart : IResponseRenderingProcPart
{
    private readonly InversionProc _inversionProc = new InversionProc();
    private readonly ServiceLocator _sl;
    private IOffscreenRenderingViewController _offscreenRenderingViewController;
    private IResponseDataNamedPipe _responseDataNamedPipe;

    public ResponseRenderingProcPart(ServiceLocator sl)
    {
        _sl = sl;
    }

    public void Activate()
    {
        _inversionProc.Register(
            () =>
            {
                _offscreenRenderingViewController = _sl.Get<IOffscreenRenderingViewController>();
                _responseDataNamedPipe = _sl.Get<IResponseDataNamedPipe>();
            },
            () =>
            {
                _responseDataNamedPipe = null;
                _offscreenRenderingViewController = null;
            });
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public void Update()
    {
    }
}

}
