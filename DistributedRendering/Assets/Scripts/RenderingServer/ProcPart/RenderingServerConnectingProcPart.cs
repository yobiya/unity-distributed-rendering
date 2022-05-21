using Cysharp.Threading.Tasks;

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private readonly ITestMessageSendUIViewController _testMessageSendUIViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        ITestMessageSendUIViewController testMessageSendUIViewController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIViewController = renderingServerConnectingUIViewController;
        _renderingServerConnectingUIViewController.OnRequestConnecting += () =>
        {
            _namedPipeClient.Connect(ConnectTimeOutTime);
            _renderingServerConnectingUIViewController.ShowConnecting();
        };

        _testMessageSendUIViewController = testMessageSendUIViewController;

        _namedPipeClient = namedPipeClient;
        _namedPipeClient.OnConnected += () =>
        {
            _renderingServerConnectingUIViewController.ShowConnected();
            _testMessageSendUIViewController.Activate();
        };
        _namedPipeClient.OnFailed += () => CreateFaildTask().Forget();

        _timerCreator = timerCreator;
    }

    public void Activate()
    {
        _renderingServerConnectingUIViewController.Activate();
    }

    public void Deactivate()
    {
        _renderingServerConnectingUIViewController.Deactivate();
    }

    private async UniTask CreateFaildTask()
    {
        _renderingServerConnectingUIViewController.ShowFailed();

        await _timerCreator.Create(FaildTextDisplayTime);

        _renderingServerConnectingUIViewController.Reset();
    }
}
