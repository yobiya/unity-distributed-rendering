using System;
using Cysharp.Threading.Tasks;

namespace GameClient
{

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private readonly ITestMessageSendUIViewController _testMessageSendUIViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;

    public event Action<byte[]> OnRecieved;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        ITestMessageSendUIViewController testMessageSendUIViewController,
        ICameraViewController cameraViewController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIViewController = renderingServerConnectingUIViewController;

        _testMessageSendUIViewController = testMessageSendUIViewController;
        _testMessageSendUIViewController.OnSend += () => _namedPipeClient.Write("Test message.");

        _namedPipeClient = namedPipeClient;
        _namedPipeClient.OnConnected += () =>
        {
            _renderingServerConnectingUIViewController.ShowConnected();
            _testMessageSendUIViewController.Activate();

            cameraViewController.OnUpdateTransform += (transform) =>
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
        };
        _namedPipeClient.OnFailed += () => CreateFaildTask().Forget();
        _namedPipeClient.OnRecieved += (bytes) => OnRecieved?.Invoke(bytes);

        _timerCreator = timerCreator;
    }

    public async UniTask Activate()
    {
        _renderingServerConnectingUIViewController.Activate();
        await StartTask();
    }

    public void Deactivate()
    {
        _renderingServerConnectingUIViewController.Deactivate();
    }

    private async UniTask CreateFaildTask()
    {
        _renderingServerConnectingUIViewController.ShowFailed();

        await _timerCreator.Create(FaildTextDisplayTime);
    }

    private async UniTask StartTask()
    {
        bool isRequestedConnectiong = false;
        _renderingServerConnectingUIViewController.OnRequestConnecting += () =>
        {
            isRequestedConnectiong = true;
        };

        await UniTask.WaitUntil(() => isRequestedConnectiong);

        _namedPipeClient.Connect(ConnectTimeOutTime);
        _renderingServerConnectingUIViewController.ShowConnecting();
    }
}

}
