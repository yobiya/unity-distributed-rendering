using Cysharp.Threading.Tasks;

namespace RenderingServer
{

public interface IGameClientConnectionProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

}
