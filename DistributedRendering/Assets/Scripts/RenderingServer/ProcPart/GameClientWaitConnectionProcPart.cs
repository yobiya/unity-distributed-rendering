using System;
using Common;

public class GameClientWaitConnectionProcPart : IGameClientWaitConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private readonly INamedPipeServer _namedPipeServer;

    public event Action OnConnected;

    public GameClientWaitConnectionProcPart(ServiceLocator sl)
    {
        _gameClientWaitConnectionUIViewControler = sl.Get<IGameClientWaitConnectionUIViewControler>();
        _namedPipeServer = sl.Get<INamedPipeServer>();

        _namedPipeServer.OnConnected += _gameClientWaitConnectionUIViewControler.ShowConnected;
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
