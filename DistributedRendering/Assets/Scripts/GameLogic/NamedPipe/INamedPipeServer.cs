using System;

public interface INamedPipeServer
{
    event Action OnConnected;

    void WaitConnection();
}
