using System;
using Common;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIController _renderingServerConnectingUIController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;
    private readonly InversionProc _inversionProc = new InversionProc();

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIController renderingServerConnectingUIController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIController = renderingServerConnectingUIController;

        _namedPipeClient = namedPipeClient;

        _timerCreator = timerCreator;
    }

    public async UniTask<INamedPipeClient.ConnectResult> ActivateAsync()
    {
        _inversionProc.Register(_renderingServerConnectingUIController.Activate, _renderingServerConnectingUIController.Deactivate);

        // ユーザーの開始入力を待つ
        bool isRequestedConnectiong = false;
        Action requestConnecting = () => isRequestedConnectiong = true;
        _inversionProc.Register(
            () => _renderingServerConnectingUIController.OnRequestConnecting += requestConnecting,
            () => _renderingServerConnectingUIController.OnRequestConnecting -= requestConnecting);
        await UniTask.WaitUntil(() => isRequestedConnectiong);

        // 接続を開始する
        _renderingServerConnectingUIController.ShowConnecting();
        var connectResult = await _namedPipeClient.ConnectAsync(ConnectTimeOutTime);
        if (connectResult == INamedPipeClient.ConnectResult.Connected)
        {
            _renderingServerConnectingUIController.ShowConnected();
        }
        else
        {
            _renderingServerConnectingUIController.ShowFailed();

            await _timerCreator.Create(FaildTextDisplayTime);
        }

        return connectResult;
    }

    public void Deactivate()
    {
        _inversionProc.Inversion();
    }
}

}
