using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface ISyncronizeRenderingProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

}
