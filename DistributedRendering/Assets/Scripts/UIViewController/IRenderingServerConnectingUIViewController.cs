using System;

public interface IRenderingServerConnectingUIViewController
{
    event Action OnRequestConnecting;

    void ShowConnecting();
    void ShowConnected();
    void ShowFailed();
    void Reset();
}
