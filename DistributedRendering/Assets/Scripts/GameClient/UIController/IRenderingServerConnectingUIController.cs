using System;

namespace GameClient
{

public interface IRenderingServerConnectingUIController
{
    event Action OnRequestConnecting;

    void Activate();
    void Deactivate();
    void ShowConnecting();
    void ShowConnected();
    void ShowFailed();
}

}
