using System;

public interface ITestMessageSendUIViewController
{
    event Action OnSend;

    void Activate();
    void Deactivate();
}
