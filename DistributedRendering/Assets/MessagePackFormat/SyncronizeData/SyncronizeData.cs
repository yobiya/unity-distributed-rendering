using MessagePack;

namespace Common
{

[MessagePackObject]
public class SyncronizeData
{
    [Key(0)] public int test { get; set; }
}

}
