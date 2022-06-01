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

    private TestMessageSendUIViewController _testMessageSendUIViewController;
    private CameraViewController _cameraViewController;

    private IObjectResolver _objectResolver;

    void Start()
    {
        var containerBuilder = new ContainerBuilder();
        {
            // SerializeFieldを登録
            containerBuilder.RegisterComponent<IRenderingServerConnectingUI>(_renderingServerConnectingUICollection);

            containerBuilder.Register<INamedPipeClient>(_ => new NamedPipeClient(".", Definisions.CommandMessageNamedPipeName), Lifetime.Singleton);
            containerBuilder.Register<IRenderingServerConnectingUIController, RenderingServerConnectingUIController>(Lifetime.Singleton);
        }

        _objectResolver = containerBuilder.Build();

        var serviceLocator = new ServiceLocator();
        {
            serviceLocator.Set<IRenderingUI>(_renderingUI);

            serviceLocator.Set<IRenderingUIController>(new RenderingUIController(serviceLocator));
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

        _renderingServerConnectingProcPart.Activate().Forget();
        var renderingProcPart = new RenderingProcPart(serviceLocator);
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
