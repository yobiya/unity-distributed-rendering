using Common;
using MessagePack;
using MessagePackFormat;

namespace RenderingServer
{

public class SyncronizeDeserializeViewController : ISyncronizeDeserializeViewController
{
    private readonly ISyncronizeView _syncronizeView;

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
