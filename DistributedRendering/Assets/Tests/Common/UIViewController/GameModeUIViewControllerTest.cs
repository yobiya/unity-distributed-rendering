using NUnit.Framework;

namespace Common
{

public class GameModeUIViewControllerTest
{
    private class UICollection : GameModeUIViewController.IUICollection
    {
        public TestButtonUIView gameClientModeButton = new TestButtonUIView();
        public TestButtonUIView renderingServerModeButton = new TestButtonUIView();

        public bool IsActive { get; set; }

        public IButtonUIView GameClientModeButton => gameClientModeButton;
        public IButtonUIView RenderingServerModeButton => renderingServerModeButton;
    }

    [Test]
    public void StartView()
    {
        var uiCollection = new UICollection();
        var uiViewController = new GameModeUIViewController(uiCollection);

        // 初期状態ではUIは有効になっている
        Assert.IsTrue(uiCollection.IsActive);
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

        // ボタンが押されたらUIは無効になる
        Assert.IsTrue(isSelectedGameClientMode);
        Assert.IsFalse(uiCollection.IsActive);
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

        // ボタンが押されたらUIは無効になる
        Assert.IsTrue(isSelectedRenderingServerMode);
        Assert.IsFalse(uiCollection.IsActive);
    }
}

}
