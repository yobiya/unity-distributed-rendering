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
}
