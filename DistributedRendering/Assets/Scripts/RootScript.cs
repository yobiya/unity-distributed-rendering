using UnityEngine;

public class RootScript : MonoBehaviour
{
    [SerializeField]
    private RenderingServerConnectingUICollection _renderingServerConnectingUICollection;

    private RenderingServerConnectingUIViewController _renderingServerConnectingUIViewController;

    void Start()
    {
        _renderingServerConnectingUIViewController = new RenderingServerConnectingUIViewController(_renderingServerConnectingUICollection);
        _renderingServerConnectingUIViewController.OnRequestConnecting += () => Debug.Log("Request connecting.");
    }

    void Update()
    {
        
    }
}
