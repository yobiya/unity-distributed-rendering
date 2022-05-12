using System;

public interface INamedPipeClient
{
    event Action OnConnected;

    void Connect(int timeOutTime);
    void Write(string text);
}
