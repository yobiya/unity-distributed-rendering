using Common;
using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace GameClient
{

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private readonly NamedPipeClientStream _recieveBinaryNamedPipe;
    private StreamWriter _pipeWriter;

    public event Action<byte[]> OnRecieved;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
        _recieveBinaryNamedPipe = new NamedPipeClientStream(serverName, Definisions.ResponseDataPipeName, PipeDirection.In);
    }

    public async UniTask<INamedPipeClient.ConnectResult> ConnectAsync(int timeOutTime)
    {
        try
        {
            await Task.WhenAll(
                _pipeClient.ConnectAsync(timeOutTime),
                _recieveBinaryNamedPipe.ConnectAsync(timeOutTime));

            _pipeWriter = new StreamWriter(_pipeClient);
            return INamedPipeClient.ConnectResult.Connected;
        }
        catch (Exception)
        {
            return INamedPipeClient.ConnectResult.TimeOut;
        }
    }
/*
    private async Task StartConnectBinaryPipe(int timeOutTime)
    {
        await Task.Run(() => 
            {
                try
                {
                    _recieveBinaryNamedPipe.Connect(timeOutTime);
                }
                catch (Exception)
                {
                    Debug.Log("Time out.");
                }
            });

        while (true)
        {
            var source = new CancellationTokenSource();
            byte[] buffer = new byte[256 * 256 * 4 * 2];
            await _recieveBinaryNamedPipe.ReadAsync(buffer, 0, buffer.Length, source.Token);

            OnRecieved?.Invoke(buffer);
        }
    }
*/
    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}

}
