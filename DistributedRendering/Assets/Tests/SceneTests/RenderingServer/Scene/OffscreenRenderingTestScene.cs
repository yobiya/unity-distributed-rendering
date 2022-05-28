using Common;
using RenderingServer;
using UnityEngine;

public class OffscreenRenderingTestScene : MonoBehaviour
{
    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    void Start()
    {
        var serviceLocator = new ServiceLocator();

        serviceLocator.Set<IOffscreenRenderingView>(_offscreenRenderingView);
        serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);

        serviceLocator.Set<IOffscreenRenderingViewController>(new OffscreenRenderingViewController(serviceLocator));
        serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));

        var offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        var debugRenderingProcPart = new DebugRenderingProcPart(serviceLocator);

        // 表示するテクスチャが準備できたらデバッグレンダリングを有効にする
        offscreenRenderingProcPart.OnActivated += debugRenderingProcPart.Activate;

        offscreenRenderingProcPart.Activate();
    }
}
