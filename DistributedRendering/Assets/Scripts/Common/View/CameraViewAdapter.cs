using UnityEngine;

namespace Common
{

public class CameraViewAdapter : BaseViewAdapter, ICameraView
{
    [SerializeField]
    private Camera _camera;

    public void SetRenderingTargetTexture(IRenderTextureView renderTexture)
    {
        _camera.targetTexture = renderTexture.RenderTexture;
    }
}

}
