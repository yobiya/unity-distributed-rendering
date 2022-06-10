using UnityEngine;
using Common;
using VContainer;
using VContainer.Unity;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public class GameClientScene : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private RenderingServerConnectingUI _renderingServerConnectingUICollection;

    [SerializeField]
    private RenderingUI _renderingUI;

    [SerializeField]
    private CameraView _cameraView;

    private IRenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private IServerRenderingProcPart _serverRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Awake()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(SystemDefinisions.DisableOnLoadTag);
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
    }

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeField
            containerBuilder.RegisterComponent<Camera>(_camera);
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<IRenderingUI>(_renderingUI);
            containerBuilder.RegisterComponent<ICameraView>(_cameraView);

            // View
            containerBuilder.Register<ISyncronizeView, SyncronizeView>(Lifetime.Scoped);

            // ViewController
            containerBuilder.Register<ISyncronizeSerializeViewController, SyncronizeSerializeViewController>(Lifetime.Scoped);

            // UIController
            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", NamedPipeDefinisions.PipeName), Lifetime.Scoped);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Scoped);
            containerBuilder.Register<IRenderingUIController, RenderingUIController>(Lifetime.Scoped);

            // GameLogic
            containerBuilder.Register<ITimerCreator, TimerCreator>(Lifetime.Scoped);
            containerBuilder.Register<ISerializer, MessagePackSerializerWrapper>(Lifetime.Scoped);

            // ProcPart
            containerBuilder.Register<IServerRenderingProcPart, ServerRenderingProcPart>(Lifetime.Scoped);
            containerBuilder.Register<IRenderingServerConnectingProcPart, RenderingServerConnectingProcPart>(Lifetime.Scoped);
        }

        _objectResolver = containerBuilder.Build();

        _renderingServerConnectingProcPart = _objectResolver.Resolve<IRenderingServerConnectingProcPart>();
        _serverRenderingProcPart = _objectResolver.Resolve<IServerRenderingProcPart>();

        StartProcPartAsync().Forget();
    }

    private async UniTask StartProcPartAsync()
    {
        while (true)
        {
            var result = await _renderingServerConnectingProcPart.ActivateAsync();
            if (result == INamedPipeClient.ConnectResult.Connected)
            {
                // 接続に成功した
                break;
            }
            else
            {
                // 接続に失敗したので、もう一度最初からやり直す
                _renderingServerConnectingProcPart.Deactivate();
            }
        }

        await _serverRenderingProcPart.ActivateAsync();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        _objectResolver.Dispose();
    }
}

}
