using System.Collections.Generic;
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
public class TransformData
{
    [Key(0)] public UnityEngine.Vector3 position;
    [Key(1)] public UnityEngine.Quaternion rotation;
    [Key(2)] public UnityEngine.Vector3 scale;
}

[MessagePackObject]
public class SyncronizeData
{
    [Key(0)] public CameraData camera = null;
    [Key(1)] public List<TransformData> transforms = null;
}

}
