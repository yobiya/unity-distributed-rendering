using NUnit.Framework;

public class GameClientWaitConnectionUIViewControllerTest
{
    class UICollection : GameClientWaitConnectionUIViewControler.IUICollection
    {
        public bool IsActive { get; set; }
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
}
