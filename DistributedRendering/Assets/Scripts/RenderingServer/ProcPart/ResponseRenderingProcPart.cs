using Common;

namespace RenderingServer
{

public class ResponseRenderingProcPart : IResponseRenderingProcPart
{
    private readonly InversionProc _inversionProc = new InversionProc();
    private readonly ServiceLocator _sl;
    private IOffscreenRenderingViewController _offscreenRenderingViewController;
    private IResponseDataNamedPipe _responseDataNamedPipe;
    private bool _isActivated = false;

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
                _isActivated = true;
            },
            () =>
            {
                _isActivated = false;
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
        if (!_isActivated)
        {
            return;
        }

        _responseDataNamedPipe.SendRenderingImage(_offscreenRenderingViewController.RenderTexture.RenderTexture);
    }
}

}
