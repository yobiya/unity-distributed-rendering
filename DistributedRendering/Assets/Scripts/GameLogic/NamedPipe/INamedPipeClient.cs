public interface INamedPipeClient
{
    bool IsConnecting { get; }

    void Connect();
    void Write(string text);
}
