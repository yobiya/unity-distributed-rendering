using System;

public interface IRenderingServerConnectingUIViewController
{
    event Action OnRequestConnecting;

    void ShowConnecting();
}
