using System.IO;
using System.IO.Pipes;

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private StreamWriter _pipeWriter;

    public bool IsConnecting { get; private set; } = false;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
    }

    public void Connect()
    {
        try
        {
            _pipeClient.Connect(3000);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
            return;
        }

        _pipeWriter = new StreamWriter(_pipeClient);

        IsConnecting = true;
    }

    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}
