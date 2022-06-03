namespace GameClient
{

public interface IServerRenderingProcPart
{
    void Activate();
    void Deactivate();
    void RenderImageBuffer(byte[] buffer);
}

}
