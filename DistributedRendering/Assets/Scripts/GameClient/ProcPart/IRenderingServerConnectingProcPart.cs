using System;

namespace GameClient
{

public interface IRenderingServerConnectingProcPart
{
    event Action<byte[]> OnRecieved;

    void Activate();
    void Deactivate();
}

}
