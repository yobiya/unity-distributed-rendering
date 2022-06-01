using UnityEngine;
using Common;

namespace GameClient
{

public class RenderingServerConnectingUICollection : MonoBehaviour, RenderingServerConnectingUIController.IUICollection
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
