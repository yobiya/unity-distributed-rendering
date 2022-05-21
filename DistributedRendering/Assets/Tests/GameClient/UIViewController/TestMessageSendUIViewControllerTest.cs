using NUnit.Framework;

public class TestMessageSendUIViewControllerTest
{
    private class UICollection : TestMessageSendUIViewController.IUICollection
    {
        public TestButtonUIView sendButton = new TestButtonUIView();

        public bool IsActive { get; set; } = true;

        public IButtonUIView SendButton => sendButton;
    }

    [Test]
    public void Activate()
    {
        var uiCollection = new UICollection();
        var sut = new TestMessageSendUIViewController(uiCollection);

        sut.Deactivate();
        sut.Activate();

        Assert.IsTrue(uiCollection.IsActive);
    }

    [Test]
    public void Deactivate()
    {
        var uiCollection = new UICollection();
        var sut = new TestMessageSendUIViewController(uiCollection);

        sut.Deactivate();

        Assert.IsFalse(uiCollection.IsActive);
    }

    [Test]
    public void Send()
    {
        var uiCollection = new UICollection();
        var sut = new TestMessageSendUIViewController(uiCollection);

        bool isSendMessage = false;
        sut.OnSend += () => isSendMessage = true;

        uiCollection.sendButton.Click();

        Assert.IsTrue(isSendMessage);
    }
}
