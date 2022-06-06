using System.Collections;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RenderingServer
{

public class DebugRenderingUIControllerTest
{
    private DebugRenderingUIControler _sut;
    private Mock<IDebugRenderingUI> _debugRenderingUIMock;
    private Mock<IOffscreenRenderingRefView> _offscreenRenderingRefViewMock;

    [SetUp]
    public void SetUp()
    {
        _debugRenderingUIMock = new Mock<IDebugRenderingUI>();
        _offscreenRenderingRefViewMock = new Mock<IOffscreenRenderingRefView>();
        _sut = new DebugRenderingUIControler(_debugRenderingUIMock.Object, _offscreenRenderingRefViewMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _debugRenderingUIMock.VerifyNoOtherCalls();
        _offscreenRenderingRefViewMock.VerifyNoOtherCalls();

        _sut = null;
        _debugRenderingUIMock = null;
        _offscreenRenderingRefViewMock = null;
    }

    private void VerifyActivate()
    {
        _offscreenRenderingRefViewMock.Verify(m => m.WaitOnActivatedAsync(), Times.Once);
        _offscreenRenderingRefViewMock.VerifyGet(m => m.RenderTexture, Times.Once);
        _debugRenderingUIMock.Verify(m => m.Activate(It.IsAny<RenderTexture>()), Times.Once);
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
        _debugRenderingUIMock.Verify(m => m.Deactivate(), Times.Once);
    });
}

}
