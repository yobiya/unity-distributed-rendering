public interface INamedPipeClient
{
    bool IsConnected { get; }

    void Connect();
    void Write(string text);
}
