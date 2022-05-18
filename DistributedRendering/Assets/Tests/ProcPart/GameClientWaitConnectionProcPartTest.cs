using Moq;
using NUnit.Framework;

public class GameClientWaitConnectionProcPartTest
{
    [Test]
    public void Activate()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var sut = new GameClientWaitConnectionProcPart(gameClientWaitConnectionUIViewControlerMock.Object);

        // 初期状態は有効になっているので、一度無効にする
        sut.Deactivate();
        sut.Activate();

        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Activate(), Times.Once);
    }

    [Test]
    public void Deactivate()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var sut = new GameClientWaitConnectionProcPart(gameClientWaitConnectionUIViewControlerMock.Object);

        sut.Deactivate();

        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Deactivate(), Times.Once);
    }
}
