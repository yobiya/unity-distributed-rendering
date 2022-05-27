using Common;
using Moq;
using NUnit.Framework;

public class GameClientWaitConnectionProcPartTest
{
    private TestCollection<GameClientWaitConnectionProcPart> CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();

        serviceLocator.RegisterMock<IGameClientWaitConnectionUIViewControler>();
        serviceLocator.RegisterMock<INamedPipeServer>();

        var gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        var namedPipeServerMock = new Mock<INamedPipeServer>();
        var sut
            = new GameClientWaitConnectionProcPart(
                serviceLocator.Get<IGameClientWaitConnectionUIViewControler>(),
                serviceLocator.Get<INamedPipeServer>());

        return new TestCollection<GameClientWaitConnectionProcPart>(sut, serviceLocator);
    }

    [Test]
    public void Activate()
    {
        var collection = CreateSUT();

        // 初期状態は有効になっているので、一度無効にする
        collection.sut.Deactivate();
        collection.sut.Activate();

        collection.serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Activate(), Times.Once);
    }

    [Test]
    public void Deactivate()
    {
        var collection = CreateSUT();

        collection.sut.Deactivate();

        collection.serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Deactivate(), Times.Once);
    }

    [Test]
    public void StartWaitClientConnection()
    {
        var collection = CreateSUT();

        collection.sut.StartWaitConnection();

        collection.serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        collection.serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);
    }

    [Test]
    public void ConnectedClient()
    {
        var collection = CreateSUT();

        collection.sut.StartWaitConnection();

        collection.serviceLocator.GetMock<INamedPipeServer>().Raise(m => m.OnConnected += null);
        collection.serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowConnected(), Times.Once);
    }
}
