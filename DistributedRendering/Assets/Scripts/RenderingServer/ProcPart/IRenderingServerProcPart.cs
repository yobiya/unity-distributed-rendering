using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface IRenderingServerProcPart
{
    UniTask Activate();
    void Deactivate();
}

}
