using System;

namespace Common
{

public interface IGameModeProcPart
{
    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;
}

}
