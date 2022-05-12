using System;

public interface INamedPipeClient
{
    event Action OnConnected;
    event Action OnFailed;

    void Connect(int timeOutTime);
    void Write(string text);
}
