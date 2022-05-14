using UnityEngine;

public class RootScript : MonoBehaviour
{
    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;
    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private NamedPipeClient _namedPipeClient;

    void Start()
    {
        _namedPipeClient = new NamedPipeClient(".", "test");
        _renderingServerConnectingUIViewController = new RenderingServerConnectingUIViewController(_renderingServerConnectingUICollection);

        _renderingServerConnectingProcPart
            = new RenderingServerConnectingProcPart(
                _renderingServerConnectingUIViewController,
                _namedPipeClient,
                new TimerCreator());
    }

    void Update()
    {
        
    }
}
