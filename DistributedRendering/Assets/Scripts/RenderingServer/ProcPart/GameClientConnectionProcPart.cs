using Cysharp.Threading.Tasks;

namespace RenderingServer
{

/*
    ゲームクライアントとの接続を確立する機能
 */
public class GameClientConnectionProcPart : IGameClientConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private readonly INamedPipeServer _namedPipeServer;

    public GameClientConnectionProcPart(IGameClientWaitConnectionUIViewControler gameClientWaitConnectionUIViewControler, INamedPipeServer namedPipeServer)
    {
        _gameClientWaitConnectionUIViewControler = gameClientWaitConnectionUIViewControler;
        _namedPipeServer = namedPipeServer;
    }

    public async UniTask ActivateAsync()
    {
        _gameClientWaitConnectionUIViewControler.Activate();
        _gameClientWaitConnectionUIViewControler.ShowWaitConnection();
        await _namedPipeServer.ActivateAsync();
        _gameClientWaitConnectionUIViewControler.ShowConnected();
    }

    public void Deactivate()
    {
        _gameClientWaitConnectionUIViewControler.Deactivate();
    }
}

}
