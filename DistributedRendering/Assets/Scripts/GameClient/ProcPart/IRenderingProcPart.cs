namespace GameClient
{

public interface IRenderingProcPart
{
    void Activate();
    void Deactivate();
    void RenderImageBuffer(byte[] buffer);
}

}
