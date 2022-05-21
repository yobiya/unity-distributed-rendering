using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NamedPipeServer : INamedPipeServer
{
    private bool _isFinished = false;

    public event Action OnConnected;

    public async Task WaitConnection()
    {
        NamedPipeServerStream pipeServer = null;
        await Task.Run(() =>
            {
                pipeServer
                    = new NamedPipeServerStream(
                        "test",
                        PipeDirection.In,
                        NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous);
                pipeServer.WaitForConnection();
            });

        OnConnected?.Invoke();

        await UniTask.Create(async () =>
            {
                using var pipeReader = new StreamReader(pipeServer);
                while (!_isFinished)
                {
                    var text = pipeReader.ReadLine();

                    if (text.Length > 0)
                    {
                        Debug.Log(text);
                    }

                    await UniTask.NextFrame();
                }

                await UniTask.CompletedTask;
            });
        
        pipeServer.Dispose();
    }

    public void Finish()
    {
        _isFinished = true;
    }
}

