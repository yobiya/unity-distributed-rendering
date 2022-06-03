using System;
using System.Collections;
using Common;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RenderingServer
{

public class RenderingServerProcPartTest
{
    private RenderingServerProcPart _sut;
    private Mock<INamedPipeServer> _namedPipeServerMock;
    private Mock<ISyncCameraViewController> _syncCameraViewControllerMock;
    private Mock<IOffscreenRenderingViewController> _offscreenRenderingViewControllerMock;
    private Mock<IDebugRenderingUIControler> _debugRenderingUIControlerMock;

    [SetUp]
    public void SetUp()
    {
        _namedPipeServerMock = new Mock<INamedPipeServer>();
        _syncCameraViewControllerMock = new Mock<ISyncCameraViewController>();
        _offscreenRenderingViewControllerMock = new Mock<IOffscreenRenderingViewController>();
        _debugRenderingUIControlerMock = new Mock<IDebugRenderingUIControler>();
        _sut = new RenderingServerProcPart(
            _namedPipeServerMock.Object,
            _syncCameraViewControllerMock.Object,
            _offscreenRenderingViewControllerMock.Object,
            _debugRenderingUIControlerMock.Object);

        _namedPipeServerMock.SetupAdd(m => m.OnRecieved += It.IsAny<Action<string>>());
    }

    [TearDown]
    public void TearDown()
    {
        _namedPipeServerMock.VerifyNoOtherCalls();
        _syncCameraViewControllerMock.VerifyNoOtherCalls();
        _offscreenRenderingViewControllerMock.VerifyNoOtherCalls();
        _debugRenderingUIControlerMock.VerifyNoOtherCalls();

        _sut = null;
        _namedPipeServerMock = null;
        _syncCameraViewControllerMock = null;
        _offscreenRenderingViewControllerMock = null;
        _debugRenderingUIControlerMock = null;
    }

    private void VerifyActivate()
    {
        _namedPipeServerMock.VerifyAdd(m => m.OnRecieved += It.IsAny<Action<string>>(), Times.Once);
        _offscreenRenderingViewControllerMock.Verify(m => m.Activate(), Times.Once);
        _offscreenRenderingViewControllerMock.VerifyGet(m => m.RenderTexture, Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.Activate(It.IsAny<IRenderTextureView>()), Times.Once);
        _syncCameraViewControllerMock.Verify(m => m.Activate(), Times.Once);
        _namedPipeServerMock.Verify(m => m.ReadCommandAsync(), Times.Once);
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
        _syncCameraViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        _offscreenRenderingViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.Deactivate(), Times.Once);
        _namedPipeServerMock.Verify(m => m.Deactivate(), Times.Once);
        _namedPipeServerMock.VerifyRemove(m => m.OnRecieved -= It.IsAny<Action<string>>(), Times.Once);
    });
}

}
