using Common;
using Moq;
using NUnit.Framework;

public class GameClientWaitConnectionProcPartTest
{
    private (GameClientWaitConnectionProcPart, MockServiceLocator) CreateSUT()
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

        return (sut, serviceLocator);
    }

    [Test]
    public void Activate()
    {
        var (sut, serviceLocator) = CreateSUT();

        // 初期状態は有効になっているので、一度無効にする
        sut.Deactivate();
        sut.Activate();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Activate(), Times.Once);
    }

    [Test]
    public void Deactivate()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.Deactivate();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Deactivate(), Times.Once);
    }

    [Test]
    public void StartWaitClientConnection()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.StartWaitConnection();

        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);
    }

    [Test]
    public void ConnectedClient()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.StartWaitConnection();

        serviceLocator.GetMock<INamedPipeServer>().Raise(m => m.OnConnected += null);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowConnected(), Times.Once);
    }
}
