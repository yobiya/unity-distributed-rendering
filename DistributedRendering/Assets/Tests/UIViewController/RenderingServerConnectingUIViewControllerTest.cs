using NUnit.Framework;

public class RenderingServerConnectingUIViewControllerTest
{
    private class UICollection : RenderingServerConnectingUIViewController.IUICollection
    {
        public TestButtonUIView connectingRequestButton = new TestButtonUIView();
        public TestTextUIView connectingText = new TestTextUIView();
        public TestTextUIView connectedText = new TestTextUIView();
        public TestTextUIView failedText = new TestTextUIView();

        public bool IsActive { get; set; }

        public IButtonUIView ConnectingRequestButton => connectingRequestButton;
        public ITextUIView ConnectingText => connectingText;
        public ITextUIView ConnectedText => connectedText;
        public ITextUIView FailedText => failedText;
    }

    [Test]
    public void Activate()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        // 初期状態は有効になっているので、一度無効してからActivateを呼び出す
        sut.Deactivate();
        sut.Activate();

        Assert.IsTrue(collection.IsActive);
        Assert.IsTrue(collection.connectingRequestButton.IsActive);
        Assert.IsFalse(collection.connectingText.IsActive);
        Assert.IsFalse(collection.connectedText.IsActive);
        Assert.IsFalse(collection.failedText.IsActive);
    }

    [Test]
    public void Deactivate()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        sut.Deactivate();

        // UIの表示は無効になる
        Assert.IsFalse(collection.IsActive);
    }

    [Test]
    public void ConnectUIViewController()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        bool isRequestConnecting = false;
        sut.OnRequestConnecting += () => isRequestConnecting = true;

        Assert.IsFalse(isRequestConnecting);

        collection.connectingRequestButton.Click();

        Assert.IsTrue(isRequestConnecting);
    }

    [Test]
    public void ShowConnecting()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        sut.ShowConnecting();

        Assert.IsFalse(collection.connectingRequestButton.IsActive);
        Assert.IsTrue(collection.connectingText.IsActive);
        Assert.IsFalse(collection.connectedText.IsActive);
        Assert.IsFalse(collection.failedText.IsActive);
    }

    [Test]
    public void ShowConnected()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        sut.ShowConnected();

        Assert.IsFalse(collection.connectingRequestButton.IsActive);
        Assert.IsFalse(collection.connectingText.IsActive);
        Assert.IsTrue(collection.connectedText.IsActive);
        Assert.IsFalse(collection.failedText.IsActive);
    }

    [Test]
    public void ShowFailed()
    {
        var collection = new UICollection();
        var sut = new RenderingServerConnectingUIViewController(collection);

        sut.ShowFailed();

        Assert.IsFalse(collection.connectingRequestButton.IsActive);
        Assert.IsFalse(collection.connectingText.IsActive);
        Assert.IsFalse(collection.connectedText.IsActive);
        Assert.IsTrue(collection.failedText.IsActive);

        sut.Reset(); // 接続ボタンを再表示させる

        Assert.IsTrue(collection.connectingRequestButton.IsActive);
        Assert.IsFalse(collection.connectingText.IsActive);
        Assert.IsFalse(collection.connectedText.IsActive);
        Assert.IsFalse(collection.failedText.IsActive);
    }
}
