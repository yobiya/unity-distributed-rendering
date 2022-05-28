using System;

namespace Common
{

public interface IGameModeUI
{
    event Action OnSelectedGameClient;
    event Action OnSelectedRenderingServer;

    void Activate();
    void Deactivate();
}

}
