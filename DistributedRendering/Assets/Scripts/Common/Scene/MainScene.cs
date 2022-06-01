using UnityEngine;
using Common;
using RenderingServer;
using GameClient;
using VContainer;
using VContainer.Unity;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private GameModeUI _gameModeUI;

    [SerializeField]
    private RenderingServerConnectingUI _renderingServerConnectingUICollection;

    [SerializeField]
    private TestMessageSendUICollection _testMessageSendUICollection;

    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private SyncCameraView _syncCameraView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    [SerializeField]
    private RenderingUI _renderingUI;

    [SerializeField]
    private CameraView _cameraView;

    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private GameClientWaitConnectionProcPart _gameClientWaitConnectionProcPart;
    private ResponseRenderingProcPart _responseRenderingProcPart;

    private TestMessageSendUIViewController _testMessageSendUIViewController;
    private CameraViewController _cameraViewController;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<IGameModeUI>(_gameModeUI);

            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
            containerBuilder.Register<IGameModeUIController, GameModeUIController>(Lifetime.Singleton);

            containerBuilder.Register<IGameModeProcPart, GameModeProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IGameModeUI>(_gameModeUI);
            serviceLocator.Set<IOffscreenRenderingView>(_offscreenRenderingView);
            serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);
            serviceLocator.Set<IRenderingUI>(_renderingUI);

            serviceLocator.Set<IOffscreenRenderingViewController>(new OffscreenRenderingViewController(serviceLocator));
            serviceLocator.Set<IGameClientWaitConnectionUIViewControler>(new GameClientWaitConnectionUIViewControler(_gameClientWaitConnectionUICollection));
            serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
            serviceLocator.Set<INamedPipeServer>(new NamedPipeServer());
            serviceLocator.Set<IResponseDataNamedPipe>(new ResponseDataNamedPipe());

            serviceLocator.Set<IRenderingUIController>(new RenderingUIController(serviceLocator));

            serviceLocator.Set<IOffscreenRenderingProcPart>(new OffscreenRenderingProcPart(serviceLocator));
            serviceLocator.Set<IDebugRenderingProcPart>(new DebugRenderingProcPart(serviceLocator));
            serviceLocator.Set<IRenderingProcPart>(new RenderingProcPart(serviceLocator));

            _responseRenderingProcPart = new ResponseRenderingProcPart(serviceLocator);
            serviceLocator.Set<IResponseRenderingProcPart>(_responseRenderingProcPart);
        }

        var gameModeProcPart = _objectResolver.Resolve<IGameModeProcPart>();

        {
            var namedPipeClient = _objectResolver.Resolve<INamedPipeClient>();
            var renderingServerConnectingUIController = _objectResolver.Resolve<IRenderingServerConnectingUIController>();
            _testMessageSendUIViewController = new TestMessageSendUIViewController(_testMessageSendUICollection);
            _cameraViewController = new CameraViewController(_cameraView);
            _renderingServerConnectingProcPart
                = new RenderingServerConnectingProcPart(
                    renderingServerConnectingUIController,
                    _testMessageSendUIViewController,
                    _cameraViewController,
                    namedPipeClient,
                    new TimerCreator());
        }

        var syncCameraViewController = new SyncCameraViewController(_syncCameraView);
        _gameClientWaitConnectionProcPart = new GameClientWaitConnectionProcPart(serviceLocator, syncCameraViewController);

        ProcPartBinder
            .Bind(
                serviceLocator,
                gameModeProcPart,
                _renderingServerConnectingProcPart,
                _gameClientWaitConnectionProcPart);
    }

    void Update()
    {
        _responseRenderingProcPart.Update();
    }

    void OnDestroy()
    {
        _objectResolver.Dispose();
    }
}
