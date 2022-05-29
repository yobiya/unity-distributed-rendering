using System;

namespace RenderingServer
{

public interface IDebugCommandMessageUI
{
    event Action OnClickedRender;

    void Activate();
    void Deactivate();
}

}
