using System;

namespace RenderingServer
{

public interface ITestCommandMessageUI
{
    event Action OnClickedRender;

    void Activate();
    void Deactivate();
}

}
