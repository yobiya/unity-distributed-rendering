using System;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using MessagePackFormat;
using VContainer;

namespace RenderingServer
{

public interface ISyncronizeRenderingProcPart
{
    UniTask ActivateAsync();
    void Deactivate();
}

public class SyncronizeRenderingProcPart : ISyncronizeRenderingProcPart
{
    private readonly INamedPipeServer _namedPipeServer;
    private readonly ISerializer _serializer;
    private readonly ISyncronizeDeserializeViewController _syncronizeDeserializerViewController;
    private readonly IOffscreenRenderingViewController _offscreenRenderingViewController;
    private readonly IDebugRenderingUIControler _debugRenderingUIControler;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public SyncronizeRenderingProcPart(
        INamedPipeServer namedPipeServer,
        ISerializer serializer,
        ISyncronizeDeserializeViewController syncronizeDeserializerViewController,
        IOffscreenRenderingViewController offscreenRenderingViewController,
        IDebugRenderingUIControler debugRenderingUIControler)
    {
        _namedPipeServer = namedPipeServer;
        _serializer = serializer;
        _syncronizeDeserializerViewController = syncronizeDeserializerViewController;
        _offscreenRenderingViewController = offscreenRenderingViewController;
        _debugRenderingUIControler = debugRenderingUIControler;
    }

    public async UniTask ActivateAsync()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        _inversionProc.RegisterInversion(cancellationTokenSource.Cancel);

        // 最初にセットアップを行う
        await SetUpCommandAsync(token);

        await _inversionProc.RegisterAsync(
            _debugRenderingUIControler.ActivateAsync(),
            _debugRenderingUIControler.Deactivate);

        while (!token.IsCancellationRequested)
        {
            var recievedData = await _namedPipeServer.RecieveDataAsync(token);

            var command = PickCommand(recievedData);
            if (command != NamedPipeDefinisions.Command.Syncronize)
            {
                // 同期コマンドのみ受け付ける
                throw new NotSupportedException($"Command '{command.ToString()}' is not supported.");
            }

            // 同期するデータをデシリアライズして、対応するオブジェクトに適用する
            _syncronizeDeserializerViewController.Deserialize(GetBody(recievedData));

            // レンダリングした結果をバイト配列に変換する
            var sendData = _offscreenRenderingViewController.Render();

            // ゲームクライアントにレンダリング結果を送る
            await _namedPipeServer.SendDataAsync(sendData, token);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    private NamedPipeDefinisions.Command PickCommand(byte[] data)
    {
        // 先頭の1バイトにコマンドが入っている
        return (NamedPipeDefinisions.Command)data[0];
    }

    private ReadOnlyMemory<byte> GetBody(byte[] data)
    {
        // コマンドの後ろのデータを返す
        return new ReadOnlyMemory<byte>(data, 1, data.Length - 1);
    }

    private async UniTask SetUpCommandAsync(CancellationToken token)
    {
        var recievedData = await _namedPipeServer.RecieveDataAsync(token);

        var command = PickCommand(recievedData);
        if (command != NamedPipeDefinisions.Command.Setup)
        {
            // セットアップコマンドのみ受け付ける
            throw new NotSupportedException($"Command '{command.ToString()}' is not supported.");
        }

        var setupData = _serializer.Deserialize<SetupData>(GetBody(recievedData));

        await _inversionProc.RegisterAsync(
            _offscreenRenderingViewController.ActivateAsync(setupData),
            _offscreenRenderingViewController.Deactivate);
    }
}

}
