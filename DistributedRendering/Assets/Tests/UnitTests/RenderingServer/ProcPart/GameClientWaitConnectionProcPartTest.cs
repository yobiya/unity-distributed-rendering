using System;
using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class GameClientWaitConnectionProcPartTest
{
    private (GameClientWaitConnectionProcPart, MockServiceLocator) CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IGameClientWaitConnectionUIViewControler>();
        serviceLocator.RegisterMock<INamedPipeServer>();

        serviceLocator.GetMock<INamedPipeServer>().SetupAdd(m => m.OnConnected += It.IsAny<Action>());

        var sut = new GameClientWaitConnectionProcPart(serviceLocator);

        sut.Activate();

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.Activate(), Times.Once);
        serviceLocator.GetMock<INamedPipeServer>().VerifyAdd(m => m.OnConnected += It.IsAny<Action>(), Times.Once);

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
    public void StartWaitClientConnection()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.StartWaitConnection();

        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }

    [Test]
    public void ConnectedClient()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.StartWaitConnection();

        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);

        serviceLocator.GetMock<INamedPipeServer>().Raise(m => m.OnConnected += null);

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowConnected(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }
}

}