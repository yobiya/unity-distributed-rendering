using Common;
using RenderingServer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class OffscreenRenderingTestScene : MonoBehaviour
{
    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

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

        serviceLocator.Set<IOffscreenRenderingViewController>(_objectResolver.Resolve<IOffscreenRenderingViewController>());
        serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));

        var offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        var debugRenderingProcPart = new DebugRenderingProcPart(serviceLocator);

        // 表示するテクスチャが準備できたらデバッグレンダリングを有効にする
        offscreenRenderingProcPart.OnActivated += debugRenderingProcPart.Activate;

        offscreenRenderingProcPart.Activate();
    }

    void OnDestroy()
    {
        _objectResolver.Dispose();
    }
}
