using System;
using Common;
using MessagePackFormat;
using VContainer;

namespace RenderingServer
{

public interface ISyncronizeDeserializeViewController
{
    void Activate();
    void Deactivate();
    void Deserialize(ReadOnlyMemory<byte> data);
}

public class SyncronizeDeserializeViewController : ISyncronizeDeserializeViewController
{
    private readonly ISyncronizeView _syncronizeView;
    private readonly ISerializer _serializer;

    [Inject]
    public SyncronizeDeserializeViewController(ISyncronizeView syncronizeView, ISerializer serializer)
    {
        _syncronizeView = syncronizeView;
        _serializer = serializer;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void Deserialize(ReadOnlyMemory<byte> data)
    {
        var syncronizeData = _serializer.Deserialize<SyncronizeData>(data);

        _syncronizeView.Camera.transform.position = syncronizeData.camera.position;
        _syncronizeView.Camera.transform.forward = syncronizeData.camera.forward;
    }
}

}
