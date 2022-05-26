namespace Common
{

public interface ICameraView : IBaseView
{
    void SetRenderingTargetTexture(IRenderTextureView texture);
}

}
