using System;

public interface INamedPipeClient
{
    event Action OnConnected;
    event Action OnFailed;
    event Action<byte[]> OnRecieved;

    void Connect(int timeOutTime);
    void Write(string text);
}
