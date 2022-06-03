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
    private RenderingServerConnectingUI _renderingServerConnectingUICollection;

    [SerializeField]
    private TestMessageSendUICollection _testMessageSendUICollection;

    [SerializeField]
    private RenderingUI _renderingUI;

    [SerializeField]
    private CameraView _cameraView;

    private IRenderingServerConnectingProcPart _renderingServerConnectingProcPart;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<TestMessageSendUIViewController.IUICollection>(_testMessageSendUICollection);
            containerBuilder.RegisterComponent<IRenderingUI>(_renderingUI);
            containerBuilder.RegisterComponent<ICameraView>(_cameraView);

            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
            containerBuilder.Register<ITestMessageSendUIViewController, TestMessageSendUIViewController>(Lifetime.Singleton);
            containerBuilder.Register<IRenderingUIController, RenderingUIController>(Lifetime.Singleton);
            containerBuilder.Register<ICameraViewController, CameraViewController>(Lifetime.Singleton);

            containerBuilder.Register<ITimerCreator, TimerCreator>(Lifetime.Singleton);

            containerBuilder.Register<IRenderingProcPart, RenderingProcPart>(Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingProcPart, RenderingServerConnectingProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        _renderingServerConnectingProcPart = _objectResolver.Resolve<IRenderingServerConnectingProcPart>();

        _renderingServerConnectingProcPart.Activate().Forget();
        var renderingProcPart = _objectResolver.Resolve<IRenderingProcPart>();
        renderingProcPart.Activate();
        _renderingServerConnectingProcPart.OnRecieved += renderingProcPart.RenderImageBuffer;
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
