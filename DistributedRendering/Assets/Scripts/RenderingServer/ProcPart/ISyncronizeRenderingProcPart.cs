using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface ISyncronizeRenderingProcPart
{
    UniTask Activate();
    void Deactivate();
}

}
