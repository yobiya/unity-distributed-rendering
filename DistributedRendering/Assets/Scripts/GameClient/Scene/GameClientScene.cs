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
    private TestMessageSendUICollection _testMessageSendUICollection;

    [SerializeField]
    private RenderingUI _renderingUI;

    [SerializeField]
    private CameraView _cameraView;

    private IRenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private IServerRenderingProcPart _serverRenderingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeField
            containerBuilder.RegisterComponent<Camera>(_camera);
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<TestMessageSendUIViewController.IUICollection>(_testMessageSendUICollection);
            containerBuilder.RegisterComponent<IRenderingUI>(_renderingUI);
            containerBuilder.RegisterComponent<ICameraView>(_cameraView);

            // View
            containerBuilder.Register<ISyncronizeView, SyncronizeView>(Lifetime.Singleton);

            // ViewController
            containerBuilder.Register<ICameraViewController, CameraViewController>(Lifetime.Singleton);
            containerBuilder.Register<ISyncronizeSerializeViewController, SyncronizeSerializeViewController>(Lifetime.Singleton);

            // UIController
            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
            containerBuilder.Register<ITestMessageSendUIViewController, TestMessageSendUIViewController>(Lifetime.Singleton);
            containerBuilder.Register<IRenderingUIController, RenderingUIController>(Lifetime.Singleton);

            // GameObject
            containerBuilder.Register<ITimerCreator, TimerCreator>(Lifetime.Singleton);

            // ProcPart
            containerBuilder.Register<IServerRenderingProcPart, ServerRenderingProcPart>(Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingProcPart, RenderingServerConnectingProcPart>(Lifetime.Singleton);
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
