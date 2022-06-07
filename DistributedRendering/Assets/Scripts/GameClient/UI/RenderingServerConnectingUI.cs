using UnityEngine;
using Common;

namespace GameClient
{

public interface IRenderingServerConnectingUI
{
    bool IsActive { get; set; }

    IButtonUIView ConnectingRequestButton { get; }
    ITextUIView ConnectingText { get; }
    ITextUIView ConnectedText { get; }
    ITextUIView FailedText { get; }
}

public class RenderingServerConnectingUI : MonoBehaviour, IRenderingServerConnectingUI
{
    [SerializeField]
    private ButtonUIViewAdapter _connectingRequestButton;

    [SerializeField]
    private TextUIViewAdapter _connectingText;

    [SerializeField]
    private TextUIViewAdapter _connectedText;

    [SerializeField]
    private TextUIViewAdapter _failedText;

    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public IButtonUIView ConnectingRequestButton => _connectingRequestButton;
    public ITextUIView ConnectingText => _connectingText;
    public ITextUIView ConnectedText => _connectedText;
    public ITextUIView FailedText => _failedText;
}

}
