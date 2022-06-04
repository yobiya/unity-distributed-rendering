using UnityEngine;

namespace Common
{

public interface ICameraView : IBaseView
{
    void SetRenderingTargetTexture(RenderTexture renderTexture);
}

}
