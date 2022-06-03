using System.Collections;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RenderingServer
{

public class GameClientConnectionProcPartTest
{
    private GameClientConnectionProcPart _sut;
    private Mock<IGameClientWaitConnectionUIViewControler> _gameClientWaitConnectionUIViewControlerMock;
    private Mock<INamedPipeServer> _namedPipeServerMock;
    private Mock<IResponseDataNamedPipe> _responseDataNamedPipeMock;

    [SetUp]
    public void SetUp()
    {
        _gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        _namedPipeServerMock = new Mock<INamedPipeServer>();
        _responseDataNamedPipeMock = new Mock<IResponseDataNamedPipe>();

        _sut
            = new GameClientConnectionProcPart(
                _gameClientWaitConnectionUIViewControlerMock.Object,
                _namedPipeServerMock.Object,
                _responseDataNamedPipeMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _responseDataNamedPipeMock.Verify(m => m.Activate(), Times.Once);

        _gameClientWaitConnectionUIViewControlerMock.VerifyNoOtherCalls();
        _namedPipeServerMock.VerifyNoOtherCalls();
        _responseDataNamedPipeMock.VerifyNoOtherCalls();

        _sut = null;
        _gameClientWaitConnectionUIViewControlerMock = null;
        _namedPipeServerMock = null;
        _responseDataNamedPipeMock = null;
    }

    // Activateが呼ばれた場合のVerifyを実行する
    private void VerifyActivate()
    {
        _responseDataNamedPipeMock.Verify(m => m.Activate(), Times.Once);
        _namedPipeServerMock.Verify(m => m.Activate(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Activate(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowWaitConnection(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowConnected(), Times.Once);
    }

    [UnityTest]
    public IEnumerator Activate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.Activate();

        VerifyActivate();
    });

    [UnityTest]
    public IEnumerator Deactivate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.Activate();
        _sut.Deactivate();

        VerifyActivate();
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Deactivate(), Times.Once);
    });
/*
    [Test]
    public void ConnectedClient()
    {
        var (sut, serviceLocator) = CreateSUT();

        serviceLocator.GetMock<INamedPipeServer>().Verify(m => m.WaitConnection(), Times.Once);
        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowWaitConnection(), Times.Once);

        serviceLocator.GetMock<IGameClientWaitConnectionUIViewControler>().Verify(m => m.ShowConnected(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }*/
}

}
