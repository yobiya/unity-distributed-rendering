using UnityEngine;

public class RootScript : MonoBehaviour
{
    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    [SerializeField]
    private GameModeUICollection _gameModeUICollection;

    private GameModeProcPart _gameModeProcPart;
    private RenderingServerConnectingProcPart _renderingServerConnectingProcPart;

    private GameModeUIViewController _gameModeUIViewController;
    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;
    private NamedPipeClient _namedPipeClient;

    void Start()
    {
        {
            _gameModeUIViewController = new GameModeUIViewController(_gameModeUICollection);
            _gameModeProcPart = new GameModeProcPart(_gameModeUIViewController);
        }

        {
            _namedPipeClient = new NamedPipeClient(".", "test");
            _renderingServerConnectingUIViewController = new RenderingServerConnectingUIViewController(_renderingServerConnectingUICollection);

            _renderingServerConnectingProcPart
                = new RenderingServerConnectingProcPart(
                    _renderingServerConnectingUIViewController,
                    _namedPipeClient,
                    new TimerCreator());
        }

        ProcPartBinder.Bind(_gameModeProcPart, _renderingServerConnectingProcPart);

        // 初期状態で使用されないものを無効にする
        _renderingServerConnectingProcPart.Deactivate();
    }

    void Update()
    {
        
    }
}
