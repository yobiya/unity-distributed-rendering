using System;
using Common;

namespace GameClient
{

public class TestMessageSendUIViewController : ITestMessageSendUIViewController
{
    public interface IUICollection
    {
        bool IsActive { get; set; }

        IButtonUIView SendButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnSend;

    public TestMessageSendUIViewController(IUICollection uiCollection)
    {
        _uiCollection = uiCollection;

        _uiCollection.SendButton.OnClicked += () => OnSend?.Invoke();
    }

    public void Activate()
    {
        _uiCollection.IsActive = true;
    }

    public void Deactivate()
    {
        _uiCollection.IsActive = false;
    }
}

}
