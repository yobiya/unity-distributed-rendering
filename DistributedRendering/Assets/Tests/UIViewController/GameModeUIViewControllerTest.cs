using NUnit.Framework;

public class GameModeUIViewControllerTest
{
    private class UICollection : GameModeUIViewController.IUICollection
    {
        public TestButtonUIView gameClientModeButton = new TestButtonUIView();
        public TestButtonUIView renderingServerModeButton = new TestButtonUIView();

        public IButtonUIView GameClientModeButton => gameClientModeButton;
        public IButtonUIView RenderingServerModeButton => renderingServerModeButton;
    }

    [Test]
    public void StartView()
    {
        var uiCollection = new UICollection();
        var uiViewController = new GameModeUIViewController(uiCollection);

        Assert.IsTrue(uiCollection.gameClientModeButton.Active);
        Assert.IsTrue(uiCollection.renderingServerModeButton.Active);
    }

    [Test]
    public void SelectGameClientMode()
    {
        bool isSelectedGameClientMode = false;

        var uiCollection = new UICollection();
        var uiViewController = new GameModeUIViewController(uiCollection);
        uiViewController.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;

        Assert.IsFalse(isSelectedGameClientMode);

        uiCollection.gameClientModeButton.Click();

        Assert.IsTrue(isSelectedGameClientMode);
    }

    [Test]
    public void SelectRenderingServerMode()
    {
        bool isSelectedRenderingServerMode = false;

        var uiCollection = new UICollection();
        var uiViewController = new GameModeUIViewController(uiCollection);
        uiViewController.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        Assert.IsFalse(isSelectedRenderingServerMode);

        uiCollection.renderingServerModeButton.Click();

        Assert.IsTrue(isSelectedRenderingServerMode);
    }
}
