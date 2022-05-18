using System;

public class RenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
{
    public interface IUICollection
    {
        bool IsActive { get; set; }

        IButtonUIView ConnectingRequestButton { get; }
        ITextUIView ConnectingText { get; }
        ITextUIView ConnectedText { get; }
        ITextUIView FailedText { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnRequestConnecting;

    public RenderingServerConnectingUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.ConnectingRequestButton.OnClicked += () => OnRequestConnecting?.Invoke();
        Reset();
    }

    public void Activate()
    {
        Reset();
    }

    public void Deactivate()
    {
        _uiCollection.IsActive = false;
    }

    public void ShowConnecting()
    {
        _uiCollection.ConnectingRequestButton.IsActive = false;
        _uiCollection.ConnectingText.IsActive = true;
        _uiCollection.ConnectedText.IsActive = false;
        _uiCollection.FailedText.IsActive = false;
    }

    public void ShowConnected()
    {
        _uiCollection.ConnectingRequestButton.IsActive = false;
        _uiCollection.ConnectingText.IsActive = false;
        _uiCollection.ConnectedText.IsActive = true;
        _uiCollection.FailedText.IsActive = false;
    }

    public void ShowFailed()
    {
        _uiCollection.ConnectingRequestButton.IsActive = false;
        _uiCollection.ConnectingText.IsActive = false;
        _uiCollection.ConnectedText.IsActive = false;
        _uiCollection.FailedText.IsActive = true;
    }

    public void Reset()
    {
        _uiCollection.IsActive = true;
        _uiCollection.ConnectingRequestButton.IsActive = true;
        _uiCollection.ConnectingText.IsActive = false;
        _uiCollection.ConnectedText.IsActive = false;
        _uiCollection.FailedText.IsActive = false;
    }
}
