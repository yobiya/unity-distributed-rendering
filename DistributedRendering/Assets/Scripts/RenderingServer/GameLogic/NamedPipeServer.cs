using Common;
using Cysharp.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

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
                Definisions.ResponseDataPipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

        _pipeReader = new StreamReader(_namedPipeServer);

        _receiveBuffer = new byte[Definisions.SyncronizeDataSize];
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

    public void SendRenderingImage(RenderTexture renderTexture)
    {
        var texture2d
            = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.ARGB32,
                false);

        RenderTexture.active = renderTexture;
        texture2d.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;
                                   
        byte[] byteArray = texture2d.GetRawTextureData();
        _namedPipeServer.Write(byteArray, 0, byteArray.Length);
        _namedPipeServer.Flush();

        Texture2D.Destroy(texture2d);
    }
}

