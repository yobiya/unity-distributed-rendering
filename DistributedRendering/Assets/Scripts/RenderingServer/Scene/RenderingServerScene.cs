using UnityEngine;
using Common;
using RenderingServer;
using VContainer;
using VContainer.Unity;

public class RenderingServerScene : MonoBehaviour
{
    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private SyncCameraView _syncCameraView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    private GameClientWaitConnectionProcPart _gameClientWaitConnectionProcPart;
    private ResponseRenderingProcPart _responseRenderingProcPart;
    private OffscreenRenderingProcPart _offscreenRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView);

            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);

            serviceLocator.Set<IOffscreenRenderingViewController>(_objectResolver.Resolve<IOffscreenRenderingViewController>());
            serviceLocator.Set<IGameClientWaitConnectionUIViewControler>(new GameClientWaitConnectionUIViewControler(_gameClientWaitConnectionUICollection));
            serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
            serviceLocator.Set<INamedPipeServer>(new NamedPipeServer());
            serviceLocator.Set<IResponseDataNamedPipe>(new ResponseDataNamedPipe());

            serviceLocator.Set<IDebugRenderingProcPart>(new DebugRenderingProcPart(serviceLocator));

            _responseRenderingProcPart = new ResponseRenderingProcPart(serviceLocator);
            serviceLocator.Set<IResponseRenderingProcPart>(_responseRenderingProcPart);
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
