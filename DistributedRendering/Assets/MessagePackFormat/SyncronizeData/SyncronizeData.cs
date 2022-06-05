using MessagePack;

namespace MessagePackFormat
{

[MessagePackObject]
public class CameraData
{
    [Key(0)] public UnityEngine.Vector3 position;
    [Key(1)] public UnityEngine.Vector3 forward;
}

[MessagePackObject]
public class SyncronizeData
{
    [Key(0)] public CameraData camera = null;
}

}
