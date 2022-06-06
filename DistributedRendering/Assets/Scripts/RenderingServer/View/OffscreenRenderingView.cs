using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RenderingServer
{

public interface IOffscreenRenderingRefView
{
    RenderTexture RenderTexture { get; }

    UniTask WaitOnActivatedAsync();
}

public interface IOffscreenRenderingView : IOffscreenRenderingRefView
{
    UniTask ActivateAsync();
    void Deactivate();
}

public class OffscreenRenderingView : MonoBehaviour, IOffscreenRenderingView
{
    private readonly InversionProc _inversionProc = new InversionProc();
    private bool _isActivated = false;

    [SerializeField]
    private Camera _camera;

    public RenderTexture RenderTexture { get; private set; }

    void Update()
    {
        _camera.Render();
    }

    public async UniTask ActivateAsync()
    {
        _inversionProc.Register(
            () => RenderTexture
                = new RenderTexture(
                    RenderingDefinisions.RenderingTextureWidth,
                    RenderingDefinisions.RenderingTextureHight,
                    16,
                    RenderTextureFormat.ARGB32),
            () => RenderTexture.Destroy(RenderTexture));

        _inversionProc.Register(
            () => gameObject.SetActive(true),
            () => gameObject.SetActive(false));

        _inversionProc.Register(
            () => _camera.targetTexture = RenderTexture,
            () => _camera.targetTexture = null);

        _inversionProc.Register(
            () => _isActivated = true,
            () => _isActivated = false);

        await UniTask.CompletedTask;
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    public async UniTask WaitOnActivatedAsync()
    {
        await UniTask.WaitUntil(() => _isActivated);
    }
}

}
