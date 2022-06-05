using Common;
using MessagePack;
using MessagePackFormat;
using VContainer;

namespace RenderingServer
{

public interface ISyncronizeDeserializeViewController
{
    void Activate();
    void Deactivate();
    void Deserialize(byte[] data);
}

public class SyncronizeDeserializeViewController : ISyncronizeDeserializeViewController
{
    private readonly ISyncronizeView _syncronizeView;

    [Inject]
    public SyncronizeDeserializeViewController(ISyncronizeView syncronizeView)
    {
        _syncronizeView = syncronizeView;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void Deserialize(byte[] data)
    {
        var syncronizeData = MessagePackSerializer.Deserialize<SyncronizeData>(data);

        _syncronizeView.Camera.transform.position = syncronizeData.camera.position;
        _syncronizeView.Camera.transform.forward = syncronizeData.camera.forward;
    }
}

}
