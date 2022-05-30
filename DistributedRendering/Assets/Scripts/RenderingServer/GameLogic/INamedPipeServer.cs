using System;
using System.Threading.Tasks;

public interface INamedPipeServer
{
    event Action OnConnected;
    event Action<string> OnRecieved;

    Task WaitConnection();
}
