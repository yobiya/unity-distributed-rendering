using UnityEngine;
using Common;
using RenderingServer;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private GameModeUI _gameModeUI;

    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    [SerializeField]
    private TestMessageSendUICollection _testMessageSendUICollection;

    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    private GameModeProcPart _gameModeProcPart;
    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private GameClientWaitConnectionProcPart _gameClientWaitConnectionProcPart;

    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private TestMessageSendUIViewController _testMessageSendUIViewController;

    void Start()
    {
        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IGameModeUI>(_gameModeUI);
            serviceLocator.Set<IOffscreenRenderingView>(_offscreenRenderingView);
            serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);

            serviceLocator.Set<IOffscreenRenderingViewController>(new OffscreenRenderingViewController(serviceLocator));
            serviceLocator.Set<IGameClientWaitConnectionUIViewControler>(new GameClientWaitConnectionUIViewControler(_gameClientWaitConnectionUICollection));
            serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
            serviceLocator.Set<INamedPipeServer>(new NamedPipeServer());

            serviceLocator.Set<IGameModeUIController>(new GameModeUIController(serviceLocator));

            serviceLocator.Set<IOffscreenRenderingProcPart>(new OffscreenRenderingProcPart(serviceLocator));
            serviceLocator.Set<IDebugRenderingProcPart>(new DebugRenderingProcPart(serviceLocator));
        }

        {
            _gameModeProcPart = new GameModeProcPart(serviceLocator);
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

        _gameClientWaitConnectionProcPart = new GameClientWaitConnectionProcPart(serviceLocator);

        ProcPartBinder
            .Bind(
                serviceLocator,
                _gameModeProcPart,
                _renderingServerConnectingProcPart,
                _gameClientWaitConnectionProcPart);

        // 初期状態で使用されないものを無効にする
        _renderingServerConnectingProcPart.Deactivate();
    }

    void Update()
    {
        
    }
}
