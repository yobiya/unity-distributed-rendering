using System;

public interface IGameModeProcPart
{
    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;
}
