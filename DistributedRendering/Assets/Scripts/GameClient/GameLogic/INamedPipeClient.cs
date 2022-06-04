using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public interface INamedPipeClient
{
    public enum ConnectResult
    {
        Connected,
        TimeOut
    }

    UniTask<ConnectResult> ConnectAsync(int timeOutTime);
    UniTask<byte[]> RecieveDataAsync(CancellationToken token);
    void Write(string text);
}

}
