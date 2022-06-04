namespace GameClient
{

public class SyncronizeSerializrViewController : ISyncronizeSerializrViewController
{
    private readonly ISyncronizeObjectHolder _objectHolder;

    public SyncronizeSerializrViewController(ISyncronizeObjectHolder objectHolder)
    {
        _objectHolder = objectHolder;
    }

    public string Create()
    {
        var camera = _objectHolder.Camera;

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
