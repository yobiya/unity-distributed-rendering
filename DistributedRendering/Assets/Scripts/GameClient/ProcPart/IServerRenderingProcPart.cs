using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface IServerRenderingProcPart
{
    UniTask Activate();
    void Deactivate();
}

}
