using Common;
using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingView : MonoBehaviour, IOffscreenRenderingView
{
    [SerializeField]
    private Camera _camera;

    public IRenderTextureView RenderTexture { get; } = new RenderTextureWrapperView();

    void Update()
    {
        _camera.Render();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        RenderTexture.Activate();

        _camera.targetTexture = RenderTexture.RenderTexture;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

}
