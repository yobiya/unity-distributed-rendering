using NUnit.Framework;

public class RenderingServerConnectingUIViewControllerTest
{
    private class UICollection : RenderingServerConnectingUIViewController.IUICollection
    {
        public TestButtonUIView connectingRequestButton = new TestButtonUIView();
        public TestTextUIView connectingText = new TestTextUIView();
        public TestTextUIView connectedText = new TestTextUIView();
        public TestTextUIView failedText = new TestTextUIView();

        public IButtonUIView ConnectingRequestButton => connectingRequestButton;
        public ITextUIView ConnectingText => connectingText;
        public ITextUIView ConnectedText => connectedText;
        public ITextUIView FailedText => failedText;
    }

    [Test]
    public void StartView()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        Assert.IsTrue(collection.connectingRequestButton.Active);
        Assert.IsFalse(collection.connectingText.Active);
        Assert.IsFalse(collection.connectedText.Active);
        Assert.IsFalse(collection.failedText.Active);
    }

    [Test]
    public void ConnectUIViewController()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        bool isRequestConnecting = false;
        controller.OnRequestConnecting += () => isRequestConnecting = true;

        Assert.IsFalse(isRequestConnecting);

        collection.connectingRequestButton.Click();

        Assert.IsTrue(isRequestConnecting);
    }

    [Test]
    public void ShowConnecting()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        controller.ShowConnecting();

        Assert.IsFalse(collection.connectingRequestButton.Active);
        Assert.IsTrue(collection.connectingText.Active);
        Assert.IsFalse(collection.connectedText.Active);
        Assert.IsFalse(collection.failedText.Active);
    }

    [Test]
    public void ShowConnected()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        controller.ShowConnected();

        Assert.IsFalse(collection.connectingRequestButton.Active);
        Assert.IsFalse(collection.connectingText.Active);
        Assert.IsTrue(collection.connectedText.Active);
        Assert.IsFalse(collection.failedText.Active);
    }

    [Test]
    public void ShowFailed()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        controller.ShowFailed();

        Assert.IsFalse(collection.connectingRequestButton.Active);
        Assert.IsFalse(collection.connectingText.Active);
        Assert.IsFalse(collection.connectedText.Active);
        Assert.IsTrue(collection.failedText.Active);
    }
}
