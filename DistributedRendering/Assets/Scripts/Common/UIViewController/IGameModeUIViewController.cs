using System;

namespace Common
{

public interface IGameModeUIViewController
{
    bool IsActive { get; set; }

    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;
}

}
