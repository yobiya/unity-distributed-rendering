using System;
using Common;

namespace GameClient
{

public class RenderingServerConnectingUIController : IRenderingServerConnectingUIController
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

    public RenderingServerConnectingUIController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.ConnectingRequestButton.OnClicked += () => OnRequestConnecting?.Invoke();
    }

    public void Activate()
    {
        _uiCollection.IsActive = true;
        _uiCollection.ConnectingRequestButton.IsActive = true;
        _uiCollection.ConnectingText.IsActive = false;
        _uiCollection.ConnectedText.IsActive = false;
        _uiCollection.FailedText.IsActive = false;
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
}

}
