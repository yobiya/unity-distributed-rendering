using Common;
using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingView : MonoBehaviour, IOffscreenRenderingView
{
    [SerializeField]
    private Camera _camera;

    public Texture tex;

    public IRenderTextureView RenderTexture { get; } = new RenderTextureWrapperView();

    void Update()
    {
        _camera.Render();
    }

    public void Activate()
    {
        RenderTexture.Activate();

        _camera.targetTexture = RenderTexture.RenderTexture;
        tex = RenderTexture.RenderTexture;
    }

    public void Deactivate()
    {
    }
}

}
