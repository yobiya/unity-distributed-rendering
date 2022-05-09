using UnityEngine;

public class RenderingServerConnectingUICollection : MonoBehaviour, RenderingServerConnectingUIViewController.IUICollection
{
    [SerializeField]
    private ButtonUIViewAdapter _connectingRequestButton;

    public IButtonUIView ConnectingRequestButton => _connectingRequestButton;
}
