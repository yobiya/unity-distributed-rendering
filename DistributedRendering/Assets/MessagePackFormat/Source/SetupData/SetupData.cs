using MessagePack;

namespace MessagePackFormat
{

[MessagePackObject]
public class SetupData
{
    [Key(0)] public UnityEngine.RectInt renderingRect;
}

}
