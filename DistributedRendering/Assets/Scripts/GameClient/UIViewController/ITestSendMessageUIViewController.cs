using System;

public interface ITestSendMessageUIViewController
{
    event Action OnSend;

    void Activate();
    void Deactivate();
}
