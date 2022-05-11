using NUnit.Framework;

public class RenderingServerConnectingUIViewControllerTest
{
    private class UICollection : RenderingServerConnectingUIViewController.IUICollection
    {
        public TestButtonUIView connectingRequestButton = new TestButtonUIView();

        public IButtonUIView ConnectingRequestButton => connectingRequestButton;
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
}
