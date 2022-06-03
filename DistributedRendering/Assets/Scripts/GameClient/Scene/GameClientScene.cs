using UnityEngine;
using Common;
using GameClient;
using VContainer;
using VContainer.Unity;
using Cysharp.Threading.Tasks;

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

    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;

    private ITestMessageSendUIViewController _testMessageSendUIViewController;
    private CameraViewController _cameraViewController;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);
            containerBuilder.RegisterComponent<TestMessageSendUIViewController.IUICollection>(_testMessageSendUICollection);
            containerBuilder.RegisterComponent<IRenderingUI>(_renderingUI);

            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
            containerBuilder.Register<ITestMessageSendUIViewController, TestMessageSendUIViewController>(Lifetime.Singleton);
            containerBuilder.Register<IRenderingUIController, RenderingUIController>(Lifetime.Singleton);

            containerBuilder.Register<IRenderingProcPart, RenderingProcPart>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
        }

        {
            var namedPipeClient = _objectResolver.Resolve<INamedPipeClient>();
            var renderingServerConnectingUIController = _objectResolver.Resolve<IRenderingServerConnectingUIController>();
            _testMessageSendUIViewController = _objectResolver.Resolve<ITestMessageSendUIViewController>();
            _cameraViewController = new CameraViewController(_cameraView);
            _renderingServerConnectingProcPart
                = new RenderingServerConnectingProcPart(
                    renderingServerConnectingUIController,
                    _testMessageSendUIViewController,
                    _cameraViewController,
                    namedPipeClient,
                    new TimerCreator());
        }

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
