using Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RenderingServer
{

public class ResponseRenderingTestScene : MonoBehaviour
{
    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    [SerializeField]
    private TestCommandMessageUI _testCommandMessageUI;

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

        serviceLocator.Set<IOffscreenRenderingView>(_offscreenRenderingView);
        serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);
        serviceLocator.Set<ITestCommandMessageUI>(_testCommandMessageUI);

        serviceLocator.Set<IOffscreenRenderingViewController>(_objectResolver.Resolve<IOffscreenRenderingViewController>());
        serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
        serviceLocator.Set<ITestCommandMessageUIController>(new TestCommandMessageUIController(serviceLocator));

        var offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        var debugRenderingProcPart = new DebugRenderingProcPart(serviceLocator);
        var testCommandMessageProcPart = new TestCommandMessageProcPart(serviceLocator);

        // 表示するテクスチャが準備できたら各機能を有効にする
        offscreenRenderingProcPart.OnActivated += (texture) =>
        {
            debugRenderingProcPart.Activate(texture);
            testCommandMessageProcPart.Activate();
        };

        offscreenRenderingProcPart.Activate();
    }

    void OnDestroy()
    {
        _objectResolver.Dispose();
    }
}

}
