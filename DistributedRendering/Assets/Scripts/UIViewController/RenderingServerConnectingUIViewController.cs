using System;

public class RenderingServerConnectingUIViewController
{
    public interface IUICollection
    {
        IButtonUIView ConnectingRequestButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnRequestConnecting;

    public RenderingServerConnectingUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.ConnectingRequestButton.OnClicked += () => OnRequestConnecting?.Invoke();
    }
}
