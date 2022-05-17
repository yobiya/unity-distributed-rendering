using UnityEngine;

public class RenderingServerConnectingUICollection : MonoBehaviour, RenderingServerConnectingUIViewController.IUICollection
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
        set => gameObject.SetActive(true);
    }

    public IButtonUIView ConnectingRequestButton => _connectingRequestButton;
    public ITextUIView ConnectingText => _connectingText;
    public ITextUIView ConnectedText => _connectedText;
    public ITextUIView FailedText => _failedText;
}
