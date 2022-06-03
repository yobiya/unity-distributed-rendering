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
    UniTask<byte[]> RecieveDataAsync();
    void Write(string text);
}

}
