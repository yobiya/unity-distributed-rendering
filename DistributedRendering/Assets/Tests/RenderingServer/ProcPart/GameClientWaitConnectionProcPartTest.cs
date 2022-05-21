using Moq;
using NUnit.Framework;

public class GameClientWaitConnectionProcPartTest
{
    [Test]
    public void Activate()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var namedPipeServerMock = new Mock<INamedPipeServer>();
        var sut
            = new GameClientWaitConnectionProcPart(
                gameClientWaitConnectionUIViewControlerMock.Object,
                namedPipeServerMock.Object);

        // 初期状態は有効になっているので、一度無効にする
        sut.Deactivate();
        sut.Activate();

        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Activate(), Times.Once);
    }

    [Test]
    public void Deactivate()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var namedPipeServerMock = new Mock<INamedPipeServer>();
        var sut
            = new GameClientWaitConnectionProcPart(
                gameClientWaitConnectionUIViewControlerMock.Object,
                namedPipeServerMock.Object);

        sut.Deactivate();

        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Deactivate(), Times.Once);
    }

    [Test]
    public void StartWaitClientConnection()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var namedPipeServerMock = new Mock<INamedPipeServer>();
        var sut
            = new GameClientWaitConnectionProcPart(
                gameClientWaitConnectionUIViewControlerMock.Object,
                namedPipeServerMock.Object);

        sut.StartWaitConnection();

        namedPipeServerMock.Verify(m => m.WaitConnection(), Times.Once);
        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowWaitConnection(), Times.Once);
    }

    [Test]
    public void ConnectedClient()
    {
        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var namedPipeServerMock = new Mock<INamedPipeServer>();
        var sut
            = new GameClientWaitConnectionProcPart(
                gameClientWaitConnectionUIViewControlerMock.Object,
                namedPipeServerMock.Object);

        sut.StartWaitConnection();

        namedPipeServerMock.Raise(m => m.OnConnected += null);

        gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowConnected(), Times.Once);
    }
}
