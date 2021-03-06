namespace Common
{

public class NamedPipeDefinisions
{
    public const string PipeName = "distributed-rendering-pipe";

    public const int SyncronizeDataSize = 1024 * 512;
    public const int RenderingDataSize = 1024 * 1024 * 2;

    public enum Command
    {
        Setup,
        Syncronize
    }
}

}
