using UnityEngine;

namespace RenderingServer
{

public class OffscreenRenderingView : MonoBehaviour, IOffscreenRenderingView
{
    [SerializeField]
    private Camera _camera;

    public RenderTexture RenderTexture { get; private set; }

    void Update()
    {
        _camera.Render();
    }

    public void Activate()
    {
        RenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        gameObject.SetActive(true);

        _camera.targetTexture = RenderTexture;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        RenderTexture.Destroy(RenderTexture);
    }
}

}
