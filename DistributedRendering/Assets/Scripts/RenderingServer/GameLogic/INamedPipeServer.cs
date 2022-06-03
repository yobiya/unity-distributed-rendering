using System;
using System.Threading.Tasks;

public interface INamedPipeServer
{
    event Action<string> OnRecieved;

    Task Activate();
    void Deactivate();
    Task ReadCommandAsync();
}
