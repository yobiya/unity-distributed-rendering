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
            containerBuilder.RegisterComponent<IOffscreenRenderingView>(_offscreenRenderingView).As<IOffscreenRenderingRefView>();
            containerBuilder.RegisterComponent<IDebugRenderingUI>(_debugRenderingUI);
            containerBuilder.RegisterComponent<GameClientWaitConnectionUIViewControler.IUICollection>(_gameClientWaitConnectionUICollection);

            // View
            containerBuilder.Register<ISyncronizeView, SyncronizeView>(Lifetime.Scoped);

            // UIController
            containerBuilder.Register<IDebugRenderingUIControler, DebugRenderingUIControler>(Lifetime.Scoped);

            // ViewController
            containerBuilder.Register<IOffscreenRenderingViewController, OffscreenRenderingViewController>(Lifetime.Scoped);
            containerBuilder.Register<IGameClientWaitConnectionUIViewControler, GameClientWaitConnectionUIViewControler>(Lifetime.Scoped);
            containerBuilder.Register<ISyncronizeDeserializeViewController, SyncronizeDeserializeViewController>(Lifetime.Scoped);

            // GameLogic
            containerBuilder.Register<INamedPipeServer, NamedPipeServer>(Lifetime.Scoped);

            // ProcPart
            containerBuilder.Register<IGameClientConnectionProcPart, GameClientConnectionProcPart>(Lifetime.Scoped);
            containerBuilder.Register<ISyncronizeRenderingProcPart, SyncronizeRenderingProcPart>(Lifetime.Scoped);
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
