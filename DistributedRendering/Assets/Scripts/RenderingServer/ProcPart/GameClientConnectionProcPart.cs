using System;
using Common;

namespace RenderingServer
{

public class GameClientConnectionProcPart : IGameClientConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private readonly INamedPipeServer _namedPipeServer;

    public event Action OnConnected;

    public GameClientConnectionProcPart(ServiceLocator sl, ISyncCameraViewController syncCameraViewController)
    {
        _gameClientWaitConnectionUIViewControler = sl.Get<IGameClientWaitConnectionUIViewControler>();
        _namedPipeServer = sl.Get<INamedPipeServer>();
        sl.Get<IResponseDataNamedPipe>().Activate();

        _namedPipeServer.OnConnected += () =>
        {
            OnConnected?.Invoke();
            _gameClientWaitConnectionUIViewControler.ShowConnected();
            syncCameraViewController.Activate();
        };

        _namedPipeServer.OnRecieved += (text) => syncCameraViewController.Sync(text);
    }

    public void Activate()
    {
        _gameClientWaitConnectionUIViewControler.Activate();
    }

    public void Deactivate()
    {
        _gameClientWaitConnectionUIViewControler.Deactivate();
    }

    public void StartWaitConnection()
    {
        var _ = _namedPipeServer.WaitConnection();
        _gameClientWaitConnectionUIViewControler.ShowWaitConnection();
    }
}

}
