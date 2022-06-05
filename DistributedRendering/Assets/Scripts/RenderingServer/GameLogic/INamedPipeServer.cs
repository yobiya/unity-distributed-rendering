using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface INamedPipeServer
{
    UniTask ActivateAsync();
    void Deactivate();
    UniTask<byte[]> RecieveDataAsync(CancellationToken token);
    void SendRenderingImage(RenderTexture renderTexture);
}
