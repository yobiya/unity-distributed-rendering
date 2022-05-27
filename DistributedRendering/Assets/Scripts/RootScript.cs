using UnityEngine;
using Common;
using RenderingServer;

public class RootScript : MonoBehaviour
{
    [SerializeField]
    private GameModeUICollection _gameModeUICollection;

    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    [SerializeField]
    private TestMessageSendUICollection _testMessageSendUICollection;

    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    [SerializeField]
    private OffscreenRenderingViewCollection _offscreenRenderingViewCollection;

    private GameModeProcPart _gameModeProcPart;
    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private GameClientWaitConnectionProcPart _gameClientWaitConnectionProcPart;
    private OffscreenRenderingProcPart _offscreenRenderingProcPart;

    private GameModeUIViewController _gameModeUIViewController;
    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private GameClientWaitConnectionUIViewControler _gameClientWaitConnectionUIViewControler;
    private TestMessageSendUIViewController _testMessageSendUIViewController;
    private OffscreenRenderingViewController _offscreenRenderingViewController;

    void Start()
    {
        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IOffscreenRenderingViewController>(new OffscreenRenderingViewController(_offscreenRenderingViewCollection));
        }

        {
            _gameModeUIViewController = new GameModeUIViewController(_gameModeUICollection);
            _gameModeProcPart = new GameModeProcPart(_gameModeUIViewController);
        }

        {
            var namedPipeClient = new NamedPipeClient(".", "test");
            _renderingServerConnectingUIViewController = new RenderingServerConnectingUIViewController(_renderingServerConnectingUICollection);
            _testMessageSendUIViewController = new TestMessageSendUIViewController(_testMessageSendUICollection);
            _renderingServerConnectingProcPart
                = new RenderingServerConnectingProcPart(
                    _renderingServerConnectingUIViewController,
                    _testMessageSendUIViewController,
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

        {
            _offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        }

        ProcPartBinder
            .Bind(
                _gameModeProcPart,
                _renderingServerConnectingProcPart,
                _gameClientWaitConnectionProcPart,
                _offscreenRenderingProcPart);

        // 初期状態で使用されないものを無効にする
        _renderingServerConnectingProcPart.Deactivate();
        _gameClientWaitConnectionProcPart.Deactivate();
    }

    void Update()
    {
        
    }
}
