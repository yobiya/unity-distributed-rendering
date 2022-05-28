using System;

namespace Common
{

public interface IGameModeProcPart
{
    event Action OnSelectedGameClientMode;
    event Action OnSelectedRenderingServerMode;

    void Activate();
    void Deactivate();
}

}
