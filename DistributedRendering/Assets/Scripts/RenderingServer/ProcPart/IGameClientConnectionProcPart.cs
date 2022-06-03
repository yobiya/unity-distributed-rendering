using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface IGameClientConnectionProcPart
{
    UniTask Activate();
    void Deactivate();
}

}
