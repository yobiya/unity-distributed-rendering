using Cysharp.Threading.Tasks;

public class GameClientWaitConnectionProcPart : IGameClientWaitConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private readonly INamedPipeServer _namedPipeServer;

    public GameClientWaitConnectionProcPart(
        IGameClientWaitConnectionUIViewControler gameClientWaitConnectionUIViewControler,
        INamedPipeServer namedPipeServer)
    {
        _gameClientWaitConnectionUIViewControler = gameClientWaitConnectionUIViewControler;
        _namedPipeServer = namedPipeServer;

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
