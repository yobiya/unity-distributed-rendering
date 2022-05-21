using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

public class NamedPipeClient : INamedPipeClient
{
    private readonly NamedPipeClientStream _pipeClient;
    private StreamWriter _pipeWriter;

    public event Action OnConnected;
    public event Action OnFailed;

    public NamedPipeClient(string serverName, string pipeName)
    {
        _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out);
    }

    public void Connect(int timeOutTime)
    {
        // 非同期処理の終了は待たない
        var _ = StartConnect(timeOutTime);
    }

    private async Task StartConnect(int timeOutTime)
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

    public void Write(string text)
    {
        _pipeWriter.WriteLine(text);
        _pipeWriter.Flush();
    }
}
