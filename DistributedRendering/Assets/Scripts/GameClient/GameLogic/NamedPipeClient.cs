using Common;
using Cysharp.Threading.Tasks;
using System;
using System.IO.Pipes;
using System.Threading;

namespace GameClient
{

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _namedPipeClient;
    private readonly byte[] _recieveBuffer;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _namedPipeClient = new NamedPipeClientStream(serverName, Definisions.ResponseDataPipeName, PipeDirection.InOut);
        _recieveBuffer = new byte[Definisions.RenderingDataSize];
    }

    public async UniTask<INamedPipeClient.ConnectResult> ConnectAsync(int timeOutTime)
    {
        try
        {
            await _namedPipeClient.ConnectAsync(timeOutTime);
            return INamedPipeClient.ConnectResult.Connected;
        }
        catch (Exception)
        {
            return INamedPipeClient.ConnectResult.TimeOut;
        }
    }

    public async UniTask SendDataAsync(byte[] data, CancellationToken token)
    {
        await _namedPipeClient.WriteAsync(data, 0, data.Length, token);
        await _namedPipeClient.FlushAsync(token);
    }

    public async UniTask<byte[]> RecieveDataAsync(CancellationToken token)
    {
        await _namedPipeClient.ReadAsync(_recieveBuffer, 0, _recieveBuffer.Length, token);

        return _recieveBuffer;
    }
}

}
