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

    [SetUp]
    public void SetUp()
    {
        _gameClientWaitConnectionUIViewControlerMock = new Mock<IGameClientWaitConnectionUIViewControler>();
        _namedPipeServerMock = new Mock<INamedPipeServer>();

        _sut
            = new GameClientConnectionProcPart(
                _gameClientWaitConnectionUIViewControlerMock.Object,
                _namedPipeServerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _gameClientWaitConnectionUIViewControlerMock.VerifyNoOtherCalls();
        _namedPipeServerMock.VerifyNoOtherCalls();

        _sut = null;
        _gameClientWaitConnectionUIViewControlerMock = null;
        _namedPipeServerMock = null;
    }

    // Activateが呼ばれた場合のVerifyを実行する
    private void VerifyActivate()
    {
        _namedPipeServerMock.Verify(m => m.ActivateAsync(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Activate(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowWaitConnection(), Times.Once);
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.ShowConnected(), Times.Once);
    }

    [UnityTest]
    public IEnumerator Activate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.ActivateAsync();

        VerifyActivate();
    });

    [UnityTest]
    public IEnumerator Deactivate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.ActivateAsync();
        _sut.Deactivate();

        VerifyActivate();
        _gameClientWaitConnectionUIViewControlerMock.Verify(m => m.Deactivate(), Times.Once);
    });
}

}
