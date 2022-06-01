using System;

namespace RenderingServer
{

public interface IGameClientConnectionProcPart
{
    event Action OnConnected;

    void Activate();
    void Deactivate();
    void StartWaitConnection();
}

}
