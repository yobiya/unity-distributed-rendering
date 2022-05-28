using System;

namespace Common
{

public interface IGameModeUIController
{
    bool IsActive { get; set; }

    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;
}

}
