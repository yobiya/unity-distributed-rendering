using NUnit.Framework;
using Common;

public class GameClientWaitConnectionUIViewControllerTest
{
    class UICollection : GameClientWaitConnectionUIViewControler.IUICollection
    {
        public bool IsActive { get; set; }
        public ITextUIView WaitConnectionText { get; } = new TestTextUIView();
        public ITextUIView ConnectedText { get; } = new TestTextUIView();
    }

    [Test]
    public void Activate()
    {
        var uiCollection = new UICollection();
        var sut = new GameClientWaitConnectionUIViewControler(uiCollection);

        sut.Deactivate();
        sut.Activate();

        Assert.IsTrue(uiCollection.IsActive);
    }

    [Test]
    public void Deactivate()
    {
        var uiCollection = new UICollection();
        var sut = new GameClientWaitConnectionUIViewControler(uiCollection);

        sut.Deactivate();

        Assert.IsFalse(uiCollection.IsActive);
    }

    [Test]
    public void ShowWaitConnection()
    {
        var uiCollection = new UICollection();
        var sut = new GameClientWaitConnectionUIViewControler(uiCollection);

        sut.ShowWaitConnection();

        Assert.IsTrue(uiCollection.WaitConnectionText.IsActive);
        Assert.IsFalse(uiCollection.ConnectedText.IsActive);
    }

    [Test]
    public void ShowConnected()
    {
        var uiCollection = new UICollection();
        var sut = new GameClientWaitConnectionUIViewControler(uiCollection);

        sut.ShowConnected();

        Assert.IsFalse(uiCollection.WaitConnectionText.IsActive);
        Assert.IsTrue(uiCollection.ConnectedText.IsActive);
    }
}
