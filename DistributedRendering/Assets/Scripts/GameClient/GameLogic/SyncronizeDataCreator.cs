namespace GameClient
{

public class SyncronizeDataCreator : ISyncronizeDataCreator
{
    private readonly ISyncronizeObjectHolder _objectHolder;

    public SyncronizeDataCreator(ISyncronizeObjectHolder objectHolder)
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
