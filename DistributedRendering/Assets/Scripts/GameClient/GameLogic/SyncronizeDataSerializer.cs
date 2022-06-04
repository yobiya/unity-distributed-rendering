namespace GameClient
{

public class SyncronizeDataSerializer : ISyncronizeDataSerializer
{
    private readonly ISyncronizeObjectHolder _objectHolder;

    public SyncronizeDataSerializer(ISyncronizeObjectHolder objectHolder)
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
