using System;
using UnityEngine;

namespace RenderingServer
{

public interface IResponseDataNamedPipe
{
    event Action OnConnected;

    void Activate();
    void Deactivate();
    void SendRenderingImage(RenderTexture renderTexture);
}

}
