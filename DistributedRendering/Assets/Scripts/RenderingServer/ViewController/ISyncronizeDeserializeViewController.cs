namespace RenderingServer
{

public interface ISyncronizeDeserializeViewController
{
    void Activate();
    void Deactivate();
    void Deserialize(byte[] data);
}

}
