using System;

public interface IRenderingServerConnectingProcPart
{
    event Action<byte[]> OnRecieved;

    void Activate();
    void Deactivate();
}
