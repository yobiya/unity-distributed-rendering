using Common;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using UnityEngine;

public class NamedPipeServer : INamedPipeServer
{
    private NamedPipeServerStream _pipeServer;

    private bool _isFinished = false;

    public event Action<string> OnRecieved;

    public async Task WaitConnection()
    {
        _pipeServer
            = new NamedPipeServerStream(
                Definisions.CommandMessageNamedPipeName,
                PipeDirection.In,
                1,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);
        await _pipeServer.WaitForConnectionAsync();
    }

    public async Task ReadCommandAsync()
    {
        using var pipeReader = new StreamReader(_pipeServer);
        while (!_isFinished)
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
                _isFinished = true;
            }
        }
    }

    public void Finish()
    {
        _isFinished = true;
    }
}

