using System;
using Common;

namespace RenderingServer
{

public interface IOffscreenRenderingProcPart
{
    event Action<IRenderTextureView> OnActivated;

    void Activate();
    void Deactivate();
}

}
