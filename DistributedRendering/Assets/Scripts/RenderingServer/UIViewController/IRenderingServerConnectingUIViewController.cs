using System;

namespace GameClient
{

public interface IRenderingServerConnectingUIViewController
{
    event Action OnRequestConnecting;

    void Activate();
    void Deactivate();
    void ShowConnecting();
    void ShowConnected();
    void ShowFailed();
}

}
