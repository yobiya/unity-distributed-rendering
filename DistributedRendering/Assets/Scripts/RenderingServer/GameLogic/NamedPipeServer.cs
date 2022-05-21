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
        using var pipeServer
            = new NamedPipeServerStream(
                "test",
                PipeDirection.In,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);
        await pipeServer.WaitForConnectionAsync();

        OnConnected?.Invoke();

        using var pipeReader = new StreamReader(pipeServer);
        while (!_isFinished)
        {
            try
            {
                var text = await pipeReader.ReadLineAsync();

                if (text.Length > 0)
                {
                    Debug.Log(text);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                _isFinished = true;
            }
        }
    }

    public void Finish()
    {
        _isFinished = true;
    }
}

