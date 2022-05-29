namespace GameClient
{

public interface IRenderingUI
{
    void Activate();
    void Deactivate();
    void SetImageBuffer(byte[] buffer);
}

}
