using System;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface IRenderingServerConnectingProcPart
{
    event Action<byte[]> OnRecieved;

    UniTask<INamedPipeClient.ConnectResult> Activate();
    void Deactivate();
}

}
