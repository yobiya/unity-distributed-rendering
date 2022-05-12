using System;
using System.IO;
using System.IO.Pipes;

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private StreamWriter _pipeWriter;

    public event Action OnConnected;
    public event Action OnFailed;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
    }

    public void Connect(int timeOutTime)
    {
        try
        {
            _pipeClient.Connect(timeOutTime);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.ToString());
            OnFailed?.Invoke();
            return;
        }

        _pipeWriter = new StreamWriter(_pipeClient);

        OnConnected?.Invoke();
    }

    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}
