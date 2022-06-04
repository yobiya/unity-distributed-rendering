using Common;
using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private readonly NamedPipeClientStream _recieveBinaryNamedPipe;
    private StreamWriter _pipeWriter;

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

    public async UniTask<byte[]> RecieveDataAsync()
    {
        var source = new CancellationTokenSource();
        byte[] buffer = new byte[256 * 256 * 4 * 2];
        await _recieveBinaryNamedPipe.ReadAsync(buffer, 0, buffer.Length, source.Token);

        return buffer;
    }

    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}

}
