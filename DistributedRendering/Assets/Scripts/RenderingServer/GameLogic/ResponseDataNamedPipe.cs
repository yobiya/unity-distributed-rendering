using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using Common;
using UnityEngine;

namespace RenderingServer
{

public class ResponseDataNamedPipe : IResponseDataNamedPipe
{
    private NamedPipeServerStream _namedPipeServer;
    private bool _isFinished = false;
    private bool _isActivated = false;

    public event Action OnConnected;

    public void Activate()
    {
        var _ = WaitConnection();
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

        var texture2D
            = new Texture2D(
                    renderTexture.width,
                    renderTexture.height,
                    TextureFormat.ARGB32,
                    false);

        Graphics.CopyTexture(renderTexture, texture2D);

        byte[] byteArray = texture2D.GetRawTextureData();
        _namedPipeServer.Write(byteArray, 0, byteArray.Length);
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
