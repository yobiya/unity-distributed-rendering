using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RenderingServer
{

public interface IResponseDataNamedPipe
{
    event Action OnConnected;

    UniTask Activate();
    void Deactivate();
    UniTask<byte[]> RecieveDataAsync(CancellationToken token);
    void SendRenderingImage(RenderTexture renderTexture);
}

}
