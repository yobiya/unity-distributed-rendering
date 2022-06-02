using System;
using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class GameClientConnectionProcPartTest
{
    private (GameClientConnectionProcPart, MockServiceLocator) CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IGameClientWaitConnectionUIViewControler>();
        serviceLocator.RegisterMock<INamedPipeServer>();
        serviceLocator.RegisterMock<IResponseDataNamedPipe>();

        serviceLocator.GetMock<INamedPipeServer>().SetupAdd(m => m.OnConnected += It.IsAny<Action>());

        var sut
            = new GameClientConnectionProcPart(
                serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Object,
                serviceLocator.GetMock<INamedPipeServer>().Object,
                serviceLocator.GetMock<IResponseDataNamedPipe>().Object);

        sut.Activate();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Activate(), Times.Once);
        serviceLocator.GetMock<IResponseDataNamedPipe>().Verify(m => m.Activate(), Times.Once);
        serviceLocator.GetMock<INamedPipeServer>().VerifyAdd(m => m.OnConnected += It.IsAny<Action>(), Times.Once);
        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);

        return (sut, serviceLocator);
    }

    [Test]
    public void Activate()
    {
        var (sut, serviceLocator) = CreateSUT();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().VerifyNoOtherCalls();
    }

    [Test]
    public void Deactivate()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.Deactivate();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Deactivate(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }

    [Test]
    public void ConnectedClient()
    {
        var (sut, serviceLocator) = CreateSUT();

        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);

        serviceLocator.GetMock<INamedPipeServer>().Raise(m => m.OnConnected += null);

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowConnected(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }
}

}
