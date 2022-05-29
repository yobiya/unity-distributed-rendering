using Common;
using RenderingServer;
using UnityEngine;

namespace RenderingServer
{

public class ResponseRenderingTestScene : MonoBehaviour
{
    [SerializeField]
    private OffscreenRenderingView _offscreenRenderingView;

    [SerializeField]
    private DebugRenderingUI _debugRenderingUI;

    [SerializeField]
    private DebugCommandMessageUI _debugCommandMessageUI;

    void Start()
    {
        var serviceLocator = new ServiceLocator();

        serviceLocator.Set<IOffscreenRenderingView>(_offscreenRenderingView);
        serviceLocator.Set<IDebugRenderingUI>(_debugRenderingUI);
        serviceLocator.Set<IDebugCommandMessageUI>(_debugCommandMessageUI);

        serviceLocator.Set<IOffscreenRenderingViewController>(new OffscreenRenderingViewController(serviceLocator));
        serviceLocator.Set<IDebugRenderingUIControler>(new DebugRenderingUIControler(serviceLocator));
        serviceLocator.Set<IDebugCommandMessageUIController>(new DebugCommandMessageUIController(serviceLocator));

        var offscreenRenderingProcPart = new OffscreenRenderingProcPart(serviceLocator);
        var debugRenderingProcPart = new DebugRenderingProcPart(serviceLocator);

        // 表示するテクスチャが準備できたらデバッグレンダリングを有効にする
        offscreenRenderingProcPart.OnActivated += debugRenderingProcPart.Activate;

        offscreenRenderingProcPart.Activate();
    }
}

}
