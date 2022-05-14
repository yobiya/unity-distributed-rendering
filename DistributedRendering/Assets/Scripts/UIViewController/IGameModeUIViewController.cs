using System;

public interface IGameModeUIViewController
{
    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;
}
