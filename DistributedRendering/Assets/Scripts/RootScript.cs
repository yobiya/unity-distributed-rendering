using UnityEngine;

public class RootScript : MonoBehaviour
{
    [SerializeField]
    private GameModeUICollection _gameModeUICollection;

    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    private GameModeProcPart _gameModeProcPart;
    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private GameClientWaitConnectionProcPart _gameClientWaitConnectionProcPart;

    private GameModeUIViewController _gameModeUIViewController;
    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private GameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;

    void Start()
    {
        {
            _gameModeUIViewController = new GameModeUIViewController(_gameModeUICollection);
            _gameModeProcPart = new GameModeProcPart(_gameModeUIViewController);
        }

        {
            var namedPipeClient = new NamedPipeClient(".", "test");
            _renderingServerConnectingUIViewController = new RenderingServerConnectingUIViewController(_renderingServerConnectingUICollection);
            _renderingServerConnectingProcPart
                = new RenderingServerConnectingProcPart(
                    _renderingServerConnectingUIViewController,
                    namedPipeClient,
                    new TimerCreator());
        }

        {
            var namedPipeServer = new NamedPipeServer();
            _gameClientWaitConnectionUIViewControler = new GameClientWaitConnectionUIViewControler(_gameClientWaitConnectionUICollection);
            _gameClientWaitConnectionProcPart
                = new GameClientWaitConnectionProcPart(
                    _gameClientWaitConnectionUIViewControler,
                    namedPipeServer);
        }

        ProcPartBinder.Bind(_gameModeProcPart, _renderingServerConnectingProcPart, _gameClientWaitConnectionProcPart);

        // 初期状態で使用されないものを無効にする
        _renderingServerConnectingProcPart.Deactivate();
        _gameClientWaitConnectionProcPart.Deactivate();
    }

    void Update()
    {
        
    }
}
