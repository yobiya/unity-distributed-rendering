using System;

public interface IRenderingServerConnectingUIViewController
{
    bool IsActive { get; set; }

    event Action OnRequestConnecting;

    void ShowConnecting();
    void ShowConnected();
    void ShowFailed();
    void Reset();
}
