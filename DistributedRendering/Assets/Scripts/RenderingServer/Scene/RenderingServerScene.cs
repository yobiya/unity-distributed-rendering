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

    private IGameClientConnectionProcPart _gameClientConnectionProcPart;
    private ResponseRenderingProcPart _responseRenderingProcPart;
    private OffscreenRenderingProcPart _offscreenRenderingProcPart;
    private IDebugRenderingProcPart _debugRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView);
            containerBuilder.RegisterComponent<IDebugRenderingUI>(_debugRenderingUI);
            containerBuilder.RegisterComponent<GameClientWaitConnectionUIViewControler.IUICollection>(_gameClientWaitConnectionUICollection);

            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Singleton);
            containerBuilder.Register<IDebugRenderingUIControler, DebugRenderingUIControler>(Lifetime.Singleton);
            containerBuilder.Register<IGameClientWaitConnectionUIViewControler, GameClientWaitConnectionUIViewControler>(Lifetime.Singleton);

            containerBuilder.Register<IResponseDataNamedPipe, ResponseDataNamedPipe>(Lifetime.Singleton);

            // ProcPartを登録
            containerBuilder.Register<IDebugRenderingProcPart, DebugRenderingProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);

            serviceLocator.Set<IOffscreenRenderingViewController>(_objectResolver.Resolve<IOffscreenRenderingViewController>());
            serviceLocator.Set<IGameClientWaitConnectionUIViewControler>(_objectResolver.Resolve<IGameClientWaitConnectionUIViewControler>());
            serviceLocator.Set<INamedPipeServer>(new NamedPipeServer());

            _responseRenderingProcPart = new ResponseRenderingProcPart(serviceLocator);
            serviceLocator.Set<IResponseRenderingProcPart>(_responseRenderingProcPart);
        }

        _debugRenderingProcPart = _objectResolver.Resolve<IDebugRenderingProcPart>();

        var syncCameraViewController = new SyncCameraViewController(_syncCameraView);
        _gameClientConnectionProcPart = new GameClientConnectionProcPart(
            serviceLocator.Get<IGameClientWaitConnectionUIViewControler>(),
            serviceLocator.Get<INamedPipeServer>(),
            _objectResolver.Resolve<IResponseDataNamedPipe>());

        // ゲームクライアントとの接続を確立する
        _gameClientConnectionProcPart.Activate();
        _gameClientConnectionProcPart.StartWaitConnection();

        _offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        _offscreenRenderingProcPart.OnActivated += _debugRenderingProcPart.Activate;
        _gameClientConnectionProcPart.OnConnected += () =>
        {
            _offscreenRenderingProcPart.Activate();
            syncCameraViewController.Activate();

            serviceLocator.Get<INamedPipeServer>().OnRecieved += (text) => syncCameraViewController.Sync(text);
        };
    }

    private void StartProcPart()
    {
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