using Common;
using MessagePack;
using MessagePackFormat;
using UnityEngine;

namespace GameClient
{

public class SyncronizeSerializeViewController : ISyncronizeSerializeViewController
{
    private readonly ISyncronizeView _syncronizeView;

    public SyncronizeSerializeViewController(ISyncronizeView syncronizeView)
    {
        _syncronizeView = syncronizeView;
    }

    public string Create()
    {
        var camera = _syncronizeView.Camera;

        var transform = camera.transform;
        return $"@camera:"
                + $"{transform.position.x},"
                + $"{transform.position.y},"
                + $"{transform.position.z},"
                + $"{transform.forward.x},"
                + $"{transform.forward.y},"
                + $"{transform.forward.z}";
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
