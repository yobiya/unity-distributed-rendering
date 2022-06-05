using UnityEngine;
using Common;
using RenderingServer;
using VContainer;
using VContainer.Unity;
using Cysharp.Threading.Tasks;

public class RenderingServerScene : MonoBehaviour
{
    [SerializeField]
    private Camera _renderingCamera;

    [SerializeField]
    private GameClientWaitConnectionUICollection _gameClientWaitConnectionUICollection;

    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private SyncCameraView _syncCameraView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    private IGameClientConnectionProcPart _gameClientConnectionProcPart;
    private ISyncronizeRenderingProcPart _syncronizeRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeField
            containerBuilder.RegisterComponent<Camera>(_renderingCamera);
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView);
            containerBuilder.RegisterComponent<IDebugRenderingUI>(_debugRenderingUI);
            containerBuilder.RegisterComponent<ISyncCameraView>(_syncCameraView);
            containerBuilder.RegisterComponent<GameClientWaitConnectionUIViewControler.IUICollection>(_gameClientWaitConnectionUICollection);

            // View
            containerBuilder.Register<ISyncronizeView, SyncronizeView>(Lifetime.Singleton);

            // UIController
            containerBuilder.Register<IDebugRenderingUIControler, DebugRenderingUIControler>(Lifetime.Singleton);

            // ViewController
            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Singleton);
            containerBuilder.Register<IGameClientWaitConnectionUIViewControler, GameClientWaitConnectionUIViewControler>(Lifetime.Singleton);
            containerBuilder.Register<ISyncCameraViewController, SyncCameraViewController>(Lifetime.Singleton);
            containerBuilder.Register<ISyncronizeDeserializeViewController, SyncronizeDeserializeViewController>(Lifetime.Singleton);

            // GameLogic
            containerBuilder.Register<INamedPipeServer, NamedPipeServer>(Lifetime.Singleton);

            // ProcPart
            containerBuilder.Register<IGameClientConnectionProcPart, GameClientConnectionProcPart>(Lifetime.Singleton);
            containerBuilder.Register<ISyncronizeRenderingProcPart, SyncronizeRenderingProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        _gameClientConnectionProcPart = _objectResolver.Resolve<IGameClientConnectionProcPart>();
        _syncronizeRenderingProcPart = _objectResolver.Resolve<ISyncronizeRenderingProcPart>();

        // ゲームクライアントとの接続を確立する
        UniTask.Defer(async () =>
            {
                await _gameClientConnectionProcPart.ActivateAsync();
                await _syncronizeRenderingProcPart.ActivateAsync();
            }).Forget();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        _objectResolver.Dispose();
    }
}
