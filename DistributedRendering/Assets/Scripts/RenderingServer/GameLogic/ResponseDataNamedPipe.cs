using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RenderingServer
{

public class ResponseDataNamedPipe : IResponseDataNamedPipe
{
    private NamedPipeServerStream _namedPipeServer;
    private bool _isFinished = false;
    private bool _isActivated = false;

    public event Action OnConnected;

    public async UniTask Activate()
    {
        await WaitConnection();
    }

    public void Deactivate()
    {
        _isActivated = false;
        _isFinished = true;
        _namedPipeServer.Dispose();
        _namedPipeServer = null;
    }

    public void SendRenderingImage(RenderTexture renderTexture)
    {
        if (!_isActivated)
        {
            return;
        }

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

    public async Task WaitConnection()
    {
        _namedPipeServer
            = new NamedPipeServerStream(
                Definisions.ResponseDataPipeName,
                PipeDirection.Out,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);
        await _namedPipeServer.WaitForConnectionAsync();

        _isActivated = true;
        OnConnected?.Invoke();
    }
}

}
