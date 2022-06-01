using System;

namespace GameClient
{

public interface ITestMessageSendUIViewController
{
    event Action OnSend;

    void Activate();
    void Deactivate();
}

}
