using Common;

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
}

}
