using Common;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private readonly NamedPipeClientStream _recieveBinaryNamedPipe;
    private StreamWriter _pipeWriter;

    public event Action OnConnected;
    public event Action OnFailed;
    public event Action<byte[]> OnRecieved;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
        _recieveBinaryNamedPipe = new NamedPipeClientStream(serverName, Definisions.ResponseDataPipeName, PipeDirection.In);
    }

    public void Connect(int timeOutTime)
    {
        // 非同期処理の終了は待たない
        var _
            = Task.WhenAll(
                StartConnectMessagePipe(timeOutTime),
                StartConnectBinaryPipe(timeOutTime));
    }

    private async Task StartConnectMessagePipe(int timeOutTime)
    {
        var mainThreadContext = SynchronizationContext.Current;
        bool isTimeOut = false;

        await Task.Run(() => 
            {
                try
                {
                    _pipeClient.Connect(timeOutTime);
                }
                catch (Exception)
                {
                    isTimeOut = true;
                }
            });

        if (!isTimeOut)
        {
            _pipeWriter = new StreamWriter(_pipeClient);

            // イベントはグラフィックの更新を行う可能性があるので、メインスレッドで呼び出す
            mainThreadContext.Post((_) => OnConnected?.Invoke(), null);
        }
        else
        {
            // イベントはグラフィックの更新を行う可能性があるので、メインスレッドで呼び出す
            mainThreadContext.Post((_) => OnFailed?.Invoke(), null);
        }
    }

    private async Task StartConnectBinaryPipe(int timeOutTime)
    {
        await Task.Run(() => 
            {
                try
                {
                    _recieveBinaryNamedPipe.Connect(timeOutTime);
                }
                catch (Exception)
                {
                    Debug.Log("Time out.");
                }
            });

        while (true)
        {
            var source = new CancellationTokenSource();
            byte[] buffer = new byte[256 * 256 * 4 * 2];
            await _recieveBinaryNamedPipe.ReadAsync(buffer, 0, buffer.Length, source.Token);

            OnRecieved?.Invoke(buffer);
        }
    }

    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}

}
