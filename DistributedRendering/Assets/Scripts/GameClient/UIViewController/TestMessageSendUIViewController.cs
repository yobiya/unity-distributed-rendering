using System;

public class TestMessageSendUIViewController : ITestMessageSendUIViewController
{
    public interface IUICollection
    {
        bool IsActive { get; set; }

        IButtonUIView SendButton { get; }
    }

    private readonly IUICollection _uiCollection;

    public event Action OnSend;

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}
