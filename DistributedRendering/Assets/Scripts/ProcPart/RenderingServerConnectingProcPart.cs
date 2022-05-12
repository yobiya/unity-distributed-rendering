using System.Threading.Tasks;

public class RenderingServerConnectingProcPart
{
    private const int ConnectTimeOutTime = 3000;

    private readonly IRenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private readonly INamedPipeClient _namedPipeClient;
    private Task _connectTask;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingServerConnectingUIViewController = renderingServerConnectingUIViewController;
        _namedPipeClient = namedPipeClient;
        _namedPipeClient.OnConnected += _renderingServerConnectingUIViewController.ShowConnected;

        _renderingServerConnectingUIViewController.OnRequestConnecting += () =>
        {
            _namedPipeClient.Connect(ConnectTimeOutTime);
            _renderingServerConnectingUIViewController.ShowConnecting();
        };
    }
}
