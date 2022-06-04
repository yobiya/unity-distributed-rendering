using Moq;
using NUnit.Framework;
using UnityEngine;

namespace RenderingServer
{

public class DebugRenderingUIControllerTest
{
    private DebugRenderingUIControler _sut;
    private Mock<IDebugRenderingUI> _debugRenderingUIMock;

    [SetUp]
    public void SetUp()
    {
        _debugRenderingUIMock = new Mock<IDebugRenderingUI>();
        _sut = new DebugRenderingUIControler(_debugRenderingUIMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _debugRenderingUIMock.VerifyNoOtherCalls();

        _sut = null;
        _debugRenderingUIMock = null;
    }

    [Test]
    public void Activate()
    {
        _sut.Activate(It.IsAny<RenderTexture>());

        // DebugRenderingUIControlerの有効化に合わせて、有効化される
        _debugRenderingUIMock.Verify(m => m.Activate(It.IsAny<RenderTexture>()), Times.Once);
    }

    [Test]
    public void Deactivate()
    {
        _sut.Activate(It.IsAny<RenderTexture>());
        _sut.Deactivate();

        // DebugRenderingUIControlerの有効化と無効化に合わせて、有効化と無効化される
        _debugRenderingUIMock.Verify(m => m.Activate(It.IsAny<RenderTexture>()), Times.Once);
        _debugRenderingUIMock.Verify(m => m.Deactivate(), Times.Once);
    }
}

}
