using Cysharp.Threading.Tasks;

namespace GameClient
{

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIController _renderingServerConnectingUIController;
    private readonly ITestMessageSendUIViewController _testMessageSendUIViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIController renderingServerConnectingUIController,
        ITestMessageSendUIViewController testMessageSendUIViewController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIController = renderingServerConnectingUIController;

        _testMessageSendUIViewController = testMessageSendUIViewController;
        _testMessageSendUIViewController.OnSend += () => _namedPipeClient.Write("Test message.");

        _namedPipeClient = namedPipeClient;

        _timerCreator = timerCreator;
    }

    public async UniTask<INamedPipeClient.ConnectResult> Activate()
    {
        _renderingServerConnectingUIController.Activate();

        // ユーザーの開始入力を待つ
        bool isRequestedConnectiong = false;
        _renderingServerConnectingUIController.OnRequestConnecting += () => isRequestedConnectiong = true;
        await UniTask.WaitUntil(() => isRequestedConnectiong);

        // 接続を開始する
        _renderingServerConnectingUIController.ShowConnecting();
        var connectResult = await _namedPipeClient.ConnectAsync(ConnectTimeOutTime);
        if (connectResult == INamedPipeClient.ConnectResult.Connected)
        {
            _renderingServerConnectingUIController.ShowConnected();
            _testMessageSendUIViewController.Activate();
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
        _renderingServerConnectingUIController.Deactivate();
    }
}

}
