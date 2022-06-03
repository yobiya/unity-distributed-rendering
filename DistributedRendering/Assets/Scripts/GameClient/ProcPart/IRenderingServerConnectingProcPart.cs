using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface IRenderingServerConnectingProcPart
{
    UniTask<INamedPipeClient.ConnectResult> Activate();
    void Deactivate();
}

}
