using Common;
using Cysharp.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace RenderingServer
{

public interface INamedPipeServer
{
    UniTask ActivateAsync();
    void Deactivate();
    UniTask<byte[]> RecieveDataAsync(CancellationToken token);
    UniTask SendDataAsync(byte[] data, CancellationToken token);
}

public class NamedPipeServer : INamedPipeServer
{
    private readonly NamedPipeServerStream _namedPipeServer;
    private readonly StreamReader _pipeReader;
    private readonly byte[] _receiveBuffer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public NamedPipeServer()
    {
        _namedPipeServer
            = new NamedPipeServerStream(
                NamedPipeDefinisions.PipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

        _pipeReader = new StreamReader(_namedPipeServer);

        _receiveBuffer = new byte[NamedPipeDefinisions.SyncronizeDataSize];
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async UniTask ActivateAsync()
    {
        await _namedPipeServer.WaitForConnectionAsync(_cancellationTokenSource.Token);
    }

    public void Deactivate()
    {
        _cancellationTokenSource.Cancel();
    }

    public async UniTask<byte[]> RecieveDataAsync(CancellationToken token)
    {
        await _namedPipeServer.ReadAsync(_receiveBuffer, 0, _receiveBuffer.Length, token);

        return _receiveBuffer;
    }

    public async UniTask SendDataAsync(byte[] data, CancellationToken token)
    {
        await _namedPipeServer.WriteAsync(data, 0, data.Length, token);
        await _namedPipeServer.FlushAsync(token);
    }
}

}
