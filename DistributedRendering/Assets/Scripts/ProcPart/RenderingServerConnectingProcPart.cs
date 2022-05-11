public class RenderingServerConnectingProcPart
{
    private readonly IRenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private readonly INamedPipeClient _namedPipeClient;

    public RenderingServerConnectingProcPart(
        IRenderingServerConnectingUIViewController renderingServerConnectingUIViewController,
        INamedPipeClient namedPipeClient)
    {
        _renderingServerConnectingUIViewController = renderingServerConnectingUIViewController;
        _namedPipeClient = namedPipeClient;

        _renderingServerConnectingUIViewController.OnRequestConnecting += () =>
        {
            _namedPipeClient.Connect();
            _renderingServerConnectingUIViewController.ShowConnecting();
        };
    }

    public void Update(float dt)
    {
    }
}
