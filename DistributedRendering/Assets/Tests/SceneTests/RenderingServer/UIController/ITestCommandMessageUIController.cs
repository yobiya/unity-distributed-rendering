using System;

namespace RenderingServer
{

public interface ITestCommandMessageUIController
{
    event Action OnRenderResuest;

    void Activate();
    void Deactivate();
}

}
