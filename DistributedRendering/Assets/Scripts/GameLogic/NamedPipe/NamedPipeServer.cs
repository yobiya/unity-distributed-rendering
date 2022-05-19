using System;

public class NamedPipeServer : INamedPipeServer
{
    public event Action OnConnected;

    public void WaitConnection()
    {
    }
}
