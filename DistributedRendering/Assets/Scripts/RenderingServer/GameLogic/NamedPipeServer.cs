using Common;
using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

public class NamedPipeServer : INamedPipeServer
{
    private readonly NamedPipeServerStream _pipeServer;
    private readonly StreamReader _pipeReader;
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

        _pipeReader = new StreamReader(_pipeServer);
    }

    public async UniTask Activate()
    {
        await _pipeServer.WaitForConnectionAsync(_cancellationTokenSource.Token);
    }

    public void Deactivate()
    {
        _cancellationTokenSource.Cancel();
    }

    public async UniTask ReadCommandAsync()
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

    public async UniTask<string> RecieveMessageAsync()
    {
        try
        {
            var text = await _pipeReader.ReadLineAsync();

            if (text.Length > 0)
            {
                Debug.Log(text);

                if (text[0] == '@')
                {
                    return text;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        return "";
    }
}

