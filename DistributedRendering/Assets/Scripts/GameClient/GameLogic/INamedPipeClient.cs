using System;
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

    event Action<byte[]> OnRecieved;

    UniTask<ConnectResult> ConnectAsync(int timeOutTime);
    UniTask RecieveDataAsync();
    void Write(string text);
}

}
