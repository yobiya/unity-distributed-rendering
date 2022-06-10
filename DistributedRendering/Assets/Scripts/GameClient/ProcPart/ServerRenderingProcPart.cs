using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using MessagePackFormat;
using UnityEngine;
using VContainer;

namespace GameClient
{

public interface IServerRenderingProcPart
{
    UniTask ActivateAsync(Camera camera);
    void Deactivate();
}

public class ServerRenderingProcPart : IServerRenderingProcPart
{
    private readonly IRenderingUIController _renderingUIController;
    private readonly ISyncronizeSerializeViewController _syncronizeSerializeViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ISerializer _serializer;
    private readonly InversionProc _inversionProc = new InversionProc();

    [Inject]
    public ServerRenderingProcPart(
        IRenderingUIController renderingUIController,
        ISyncronizeSerializeViewController syncronizeSerializeViewController,
        INamedPipeClient namedPipeClient,
        ISerializer serializer)
    {
        _renderingUIController = renderingUIController;
        _syncronizeSerializeViewController = syncronizeSerializeViewController;
        _namedPipeClient = namedPipeClient;
        _serializer = serializer;
    }

    public async UniTask ActivateAsync(Camera camera)
    {
        var setupData = new SetupData();
        setupData.renderingRect = new RectInt(RenderingDefinisions.RenderingTextureWidth / 2, 0, RenderingDefinisions.RenderingTextureWidth / 2, RenderingDefinisions.RenderingTextureHight);

        _inversionProc.Register(
            () => _renderingUIController.Activate(camera, setupData),
            _renderingUIController.Deactivate);

        var cancellationTokenSource = new CancellationTokenSource();
        _inversionProc.RegisterInversion(cancellationTokenSource.Cancel);
        var token = cancellationTokenSource.Token;

        await SendSetupCommand(setupData, token);

        while (!token.IsCancellationRequested)
        {
            var sendData = _syncronizeSerializeViewController.Serialize();
            var commandData = new byte[sendData.Length + 1];
            sendData.CopyTo(commandData, 1);
            commandData[0] = (byte)NamedPipeDefinisions.Command.Syncronize;
            await _namedPipeClient.SendDataAsync(commandData, token);

            // 自分の担当する範囲の描画を行う
            _renderingUIController.RenderBaseImage();

            // サーバーから受け取った画像を表示する
            var recievedData = await _namedPipeClient.RecieveDataAsync(token);
            _renderingUIController.MargeImage(recievedData);

            await UniTask.NextFrame();
        }
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }

    private async UniTask SendSetupCommand(SetupData setupData, CancellationToken token)
    {
        var data = _serializer.Serialize(setupData);
        var commandData = new byte[data.Length + 1];
        commandData[0] = (byte)NamedPipeDefinisions.Command.Setup;
        data.CopyTo(commandData, 1);
        await _namedPipeClient.SendDataAsync(commandData, token);
    }
}

}
