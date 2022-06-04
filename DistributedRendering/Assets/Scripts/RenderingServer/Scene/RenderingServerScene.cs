using UnityEngine;
using Common;
using RenderingServer;
using VContainer;
using VContainer.Unity;
using Cysharp.Threading.Tasks;

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
    private ISyncronizeRenderingProcPart _syncronizeRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView);
            containerBuilder.RegisterComponent<IDebugRenderingUI>(_debugRenderingUI);
            containerBuilder.RegisterComponent<ISyncCameraView>(_syncCameraView);
            containerBuilder.RegisterComponent<GameClientWaitConnectionUIViewControler.IUICollection>(_gameClientWaitConnectionUICollection);

            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Singleton);
            containerBuilder.Register<IDebugRenderingUIControler, DebugRenderingUIControler>(Lifetime.Singleton);
            containerBuilder.Register<IGameClientWaitConnectionUIViewControler, GameClientWaitConnectionUIViewControler>(Lifetime.Singleton);
            containerBuilder.Register<ISyncCameraViewController, SyncCameraViewController>(Lifetime.Singleton);

            containerBuilder.Register<IResponseDataNamedPipe, ResponseDataNamedPipe>(Lifetime.Singleton);
            containerBuilder.Register<INamedPipeServer, NamedPipeServer>(Lifetime.Singleton);

            // ProcPartを登録
            containerBuilder.Register<IGameClientConnectionProcPart, GameClientConnectionProcPart>(Lifetime.Singleton);
            containerBuilder.Register<ISyncronizeRenderingProcPart, SyncronizeRenderingProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            _responseRenderingProcPart = new ResponseRenderingProcPart(serviceLocator);
            serviceLocator.Set<IResponseRenderingProcPart>(_responseRenderingProcPart);
        }

        _gameClientConnectionProcPart = _objectResolver.Resolve<IGameClientConnectionProcPart>();
        _syncronizeRenderingProcPart = _objectResolver.Resolve<ISyncronizeRenderingProcPart>();

        // ゲームクライアントとの接続を確立する
        UniTask.Defer(async () =>
            {
                await _gameClientConnectionProcPart.Activate();
                await _syncronizeRenderingProcPart.Activate();
            }).Forget();
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
