using System;
using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface IGameClientConnectionProcPart
{
    event Action OnConnected;

    UniTask Activate();
    void Deactivate();
    UniTask ReadCommandAsync();
}

}
