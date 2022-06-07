using Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace RenderingServer
{

public interface IGameClientConnectionProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

/*
    ゲームクライアントとの接続を確立する機能
 */
public class GameClientConnectionProcPart : IGameClientConnectionProcPart
{
    private readonly IGameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private readonly INamedPipeServer _namedPipeServer;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public GameClientConnectionProcPart(IGameClientWaitConnectionUIViewControler gameClientWaitConnectionUIViewControler, INamedPipeServer namedPipeServer)
    {
        _gameClientWaitConnectionUIViewControler = gameClientWaitConnectionUIViewControler;
        _namedPipeServer = namedPipeServer;
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(_gameClientWaitConnectionUIViewControler.Activate, _gameClientWaitConnectionUIViewControler.Deactivate);
        _gameClientWaitConnectionUIViewControler.ShowWaitConnection();

        await _inversionProc.RegisterAsync(_namedPipeServer.ActivateAsync(), _namedPipeServer.Deactivate);
        _gameClientWaitConnectionUIViewControler.ShowConnected();
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
