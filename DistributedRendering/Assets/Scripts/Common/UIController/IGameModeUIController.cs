using System;

namespace Common
{

public interface IGameModeUIController
{
    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;

    void Activate();
    void Deactivate();
}

}
