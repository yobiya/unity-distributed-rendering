using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RenderingServer
{

public interface IResponseDataNamedPipe
{
    event Action OnConnected;

    UniTask Activate();
    void Deactivate();
    UniTask<byte[]> RecieveDataAsync();
    void SendRenderingImage(RenderTexture renderTexture);
}

}
