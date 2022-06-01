using UnityEngine;
using Common;
using RenderingServer;
using GameClient;
using VContainer;
using VContainer.Unity;

public class RenderingServerScene : MonoBehaviour
{
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
    private OffscreenRenderingProcPart _offscreenRenderingProcPart;

    private TestMessageSendUIViewController _testMessageSendUIViewController;
    private CameraViewController _cameraViewController;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView);

            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);
            serviceLocator.Set<IRenderingUI>(_renderingUI);

            serviceLocator.Set<IOffscreenRenderingViewController>(_objectResolver.Resolve<IOffscreenRenderingViewController>());
            serviceLocator.Set<IGameClientWaitConnectionUIViewControler>(new GameClientWaitConnectionUIViewControler(_gameClientWaitConnectionUICollection));
            serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
            serviceLocator.Set<INamedPipeServer>(new NamedPipeServer());
            serviceLocator.Set<IResponseDataNamedPipe>(new ResponseDataNamedPipe());

            serviceLocator.Set<IRenderingUIController>(new RenderingUIController(serviceLocator));

            serviceLocator.Set<IDebugRenderingProcPart>(new DebugRenderingProcPart(serviceLocator));
            serviceLocator.Set<IRenderingProcPart>(new RenderingProcPart(serviceLocator));

            _responseRenderingProcPart = new ResponseRenderingProcPart(serviceLocator);
            serviceLocator.Set<IResponseRenderingProcPart>(_responseRenderingProcPart);
        }

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

        _gameClientWaitConnectionProcPart.Activate();
        _gameClientWaitConnectionProcPart.StartWaitConnection();

        _offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        _offscreenRenderingProcPart.OnActivated += serviceLocator.Get<IDebugRenderingProcPart>().Activate;
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
