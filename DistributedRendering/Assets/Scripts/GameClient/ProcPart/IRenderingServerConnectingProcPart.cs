using System;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface IRenderingServerConnectingProcPart
{
    event Action<byte[]> OnRecieved;

    UniTask Activate();
    void Deactivate();
}

}
