using System;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIController _renderingServerConnectingUIController;
    private readonly ITestMessageSendUIViewController _testMessageSendUIViewController;
    private readonly ICameraViewController _cameraViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;

    public event Action<byte[]> OnRecieved;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIController renderingServerConnectingUIController,
        ITestMessageSendUIViewController testMessageSendUIViewController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIController = renderingServerConnectingUIController;
        _cameraViewController = cameraViewController;

        _testMessageSendUIViewController = testMessageSendUIViewController;
        _testMessageSendUIViewController.OnSend += () => _namedPipeClient.Write("Test message.");

        _namedPipeClient = namedPipeClient;
        _namedPipeClient.OnRecieved += (bytes) => OnRecieved?.Invoke(bytes);

        _timerCreator = timerCreator;
    }

    public async UniTask Activate()
    {
        _renderingServerConnectingUIController.Activate();
        await StartAsync();
    }

    public void Deactivate()
    {
        _renderingServerConnectingUIController.Deactivate();
    }

    private async UniTask StartAsync()
    {
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

            _cameraViewController.OnUpdateTransform += (transform) =>
            {
                string text
                    = $"@camera:"
                    + $"{transform.position.x},"
                    + $"{transform.position.y},"
                    + $"{transform.position.z},"
                    + $"{transform.forward.x},"
                    + $"{transform.forward.y},"
                    + $"{transform.forward.z}";
                _namedPipeClient.Write(text);
            };
        }
        else
        {
            _renderingServerConnectingUIController.ShowFailed();

            await _timerCreator.Create(FaildTextDisplayTime);
        }
    }
}

}
