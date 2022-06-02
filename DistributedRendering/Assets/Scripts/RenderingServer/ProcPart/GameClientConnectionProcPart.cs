using System;
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
    private readonly IResponseDataNamedPipe _responseDataNamedPipe;

    public event Action OnConnected;

    public GameClientConnectionProcPart(IGameClientWaitConnectionUIViewControler gameClientWaitConnectionUIViewControler, INamedPipeServer namedPipeServer, IResponseDataNamedPipe responseDataNamedPipe)
    {
        _gameClientWaitConnectionUIViewControler = gameClientWaitConnectionUIViewControler;
        _namedPipeServer = namedPipeServer;
        _responseDataNamedPipe = responseDataNamedPipe;
        _responseDataNamedPipe.Activate();

        _namedPipeServer.OnConnected += () =>
        {
            OnConnected?.Invoke();
            _gameClientWaitConnectionUIViewControler.ShowConnected();
        };
    }

    public async UniTask Activate()
    {
        _gameClientWaitConnectionUIViewControler.Activate();
        _gameClientWaitConnectionUIViewControler.ShowWaitConnection();
        await _namedPipeServer.WaitConnection();
    }

    public void Deactivate()
    {
        _gameClientWaitConnectionUIViewControler.Deactivate();
    }
}

}
