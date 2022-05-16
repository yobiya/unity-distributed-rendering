using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class RenderingServerConnectingProcPart : IRenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;
    private const float FaildTextDisplayTime = 3.0f;

    private readonly IRenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private readonly ITimerCreator _timerCreator;
    private Task _connectTask;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        INamedPipeClient namedPipeClient,
        ITimerCreator timerCreator)
    {
        _renderingServerConnectingUIViewController = renderingServerConnectingUIViewController;
        _renderingServerConnectingUIViewController.OnRequestConnecting += () =>
        {
            _namedPipeClient.Connect(ConnectTimeOutTime);
            _renderingServerConnectingUIViewController.ShowConnecting();
        };

        _namedPipeClient = namedPipeClient;
        _namedPipeClient.OnConnected += _renderingServerConnectingUIViewController.ShowConnected;
        _namedPipeClient.OnFailed += () => CreateFaildTask().Forget();

        _timerCreator = timerCreator;
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    private async UniTask CreateFaildTask()
    {
        _renderingServerConnectingUIViewController.ShowFailed();

        await _timerCreator.Create(FaildTextDisplayTime);

        _renderingServerConnectingUIViewController.Reset();
    }
}
