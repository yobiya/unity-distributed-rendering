using Common;
using MessagePack;
using MessagePackFormat;
using VContainer;

namespace GameClient
{

public class SyncronizeSerializeViewController : ISyncronizeSerializeViewController
{
    private readonly ISyncronizeView _syncronizeView;

    [Inject]
    public SyncronizeSerializeViewController(ISyncronizeView syncronizeView)
    {
        _syncronizeView = syncronizeView;
    }

    public byte[] Serialize()
    {
        var data = new SyncronizeData();
        data.camera = new CameraData();
        data.camera.position = _syncronizeView.Camera.transform.position;
        data.camera.forward = _syncronizeView.Camera.transform.forward;

        return MessagePackSerializer.Serialize(data);
    }
}

}
