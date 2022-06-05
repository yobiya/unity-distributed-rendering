using Common;
using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingView : MonoBehaviour, IOffscreenRenderingView
{
    private readonly InversionProc _inversionProc = new InversionProc();

    [SerializeField]
    private Camera _camera;

    public RenderTexture RenderTexture { get; private set; }

    void Update()
    {
        _camera.Render();
    }

    public void Activate()
    {
        _inversionProc.Register(
            () => RenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32),
            () => RenderTexture.Destroy(RenderTexture));

        _inversionProc.Register(
            () => gameObject.SetActive(true),
            () => gameObject.SetActive(false));

        _inversionProc.Register(
            () => _camera.targetTexture = RenderTexture,
            () => _camera.targetTexture = null);
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
