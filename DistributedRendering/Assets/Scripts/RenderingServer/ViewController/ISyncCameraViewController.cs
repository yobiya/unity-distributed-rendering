namespace RenderingServer
{

public interface ISyncCameraViewController
{
    void Activate();
    void Deactivate();
    void Sync(string text);
}

}
