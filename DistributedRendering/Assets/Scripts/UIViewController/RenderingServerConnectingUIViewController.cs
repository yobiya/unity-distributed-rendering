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

    public bool IsActive
    {
        get => _uiCollection.IsActive;
        set => _uiCollection.IsActive = value;
    }

    public event Action OnRequestConnecting;

    public RenderingServerConnectingUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.ConnectingRequestButton.OnClicked += () => OnRequestConnecting?.Invoke();
        Reset();
    }

    public void ShowWaitUserInput()
    {
        Reset();
    }

    public void ShowConnecting()
    {
        _uiCollection.ConnectingRequestButton.Active = false;
        _uiCollection.ConnectingText.Active = true;
        _uiCollection.ConnectedText.Active = false;
        _uiCollection.FailedText.Active = false;
    }

    public void ShowConnected()
    {
        _uiCollection.ConnectingRequestButton.Active = false;
        _uiCollection.ConnectingText.Active = false;
        _uiCollection.ConnectedText.Active = true;
        _uiCollection.FailedText.Active = false;
    }

    public void ShowFailed()
    {
        _uiCollection.ConnectingRequestButton.Active = false;
        _uiCollection.ConnectingText.Active = false;
        _uiCollection.ConnectedText.Active = false;
        _uiCollection.FailedText.Active = true;
    }

    public void Reset()
    {
        _uiCollection.ConnectingRequestButton.Active = true;
        _uiCollection.ConnectingText.Active = false;
        _uiCollection.ConnectedText.Active = false;
        _uiCollection.FailedText.Active = false;
    }
}
