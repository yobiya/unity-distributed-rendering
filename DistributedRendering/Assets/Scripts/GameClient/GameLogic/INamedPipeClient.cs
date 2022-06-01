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
    void Write(string text);
}

}
