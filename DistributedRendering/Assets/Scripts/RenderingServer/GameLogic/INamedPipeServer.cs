using System;
using Cysharp.Threading.Tasks;

public interface INamedPipeServer
{
    event Action<string> OnRecieved;

    UniTask Activate();
    void Deactivate();
    UniTask ReadCommandAsync();
    UniTask<string> RecieveMessageAsync();
}
