using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface IServerRenderingProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

}
