using NUnit.Framework;

public class RenderingServerConnectingUIViewControllerTest
{
    private class UICollection : RenderingServerConnectingUIViewController.IUICollection
    {
        public TestButtonUIView connectingRequestButton = new TestButtonUIView();
        public TestTextUIView connectingTextButton = new TestTextUIView();
        public TestTextUIView connectedTextButton = new TestTextUIView();
        public TestTextUIView failedTextButton = new TestTextUIView();

        public IButtonUIView ConnectingRequestButton => connectingRequestButton;
        public ITextUIView ConnectingText => connectingTextButton;
        public ITextUIView ConnectedText => connectedTextButton;
        public ITextUIView FailedText => failedTextButton;
    }

    [Test]
    public void StartView()
    {
        var collection = new UICollection();
        var controller = new RenderingServerConnectingUIViewController(collection);

        Assert.IsTrue(collection.connectingRequestButton.Active);
        Assert.IsFalse(collection.connectingTextButton.Active);
        Assert.IsFalse(collection.connectedTextButton.Active);
        Assert.IsFalse(collection.failedTextButton.Active);
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
        Assert.IsTrue(collection.connectingTextButton.Active);
        Assert.IsFalse(collection.connectedTextButton.Active);
        Assert.IsFalse(collection.failedTextButton.Active);
    }
}
