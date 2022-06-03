using Common;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NamedPipeServer : INamedPipeServer
{
    private readonly NamedPipeServerStream _pipeServer;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public event Action<string> OnRecieved;

    public NamedPipeServer()
    {
        _pipeServer
            = new NamedPipeServerStream(
                Definisions.CommandMessageNamedPipeName,
                PipeDirection.In,
                1,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);
    }

    public async Task Activate()
    {
        await _pipeServer.WaitForConnectionAsync(_cancellationTokenSource.Token);
    }

    public void Deactivate()
    {
        _cancellationTokenSource.Cancel();
    }

    public async Task ReadCommandAsync()
    {
        using var pipeReader = new StreamReader(_pipeServer);
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                var text = await pipeReader.ReadLineAsync();

                if (text.Length > 0)
                {
                    Debug.Log(text);

                    if (text[0] == '@')
                    {
                        OnRecieved?.Invoke(text);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}

