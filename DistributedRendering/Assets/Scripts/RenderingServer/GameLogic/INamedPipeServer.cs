using System;
using System.Threading.Tasks;

public interface INamedPipeServer
{
    event Action OnConnected;

    Task WaitConnection();
}
