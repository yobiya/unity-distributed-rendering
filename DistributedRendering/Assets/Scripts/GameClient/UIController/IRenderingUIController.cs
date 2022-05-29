namespace GameClient
{

public interface IRenderingUIController
{
    void Activate();
    void Deactivate();
    void RenderImageBuffer(byte[] buffer);
}

}
