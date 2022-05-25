using System;

public interface IGameClientWaitConnectionProcPart
{
    event Action OnConnected;

    void Activate();
    void Deactivate();
    void StartWaitConnection();
}
