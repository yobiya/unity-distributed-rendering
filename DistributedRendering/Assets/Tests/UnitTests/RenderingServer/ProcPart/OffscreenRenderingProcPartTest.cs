using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class OffscreenRenderingProcPartTest
{
    private OffscreenRenderingProcPart _sut;
    private Mock<IOffscreenRenderingViewController> _offscreenRenderingViewControllerMock;

    [SetUp]
    public void SetUp()
    {
        _offscreenRenderingViewControllerMock = new Mock<IOffscreenRenderingViewController>();
        _sut = new OffscreenRenderingProcPart(_offscreenRenderingViewControllerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _offscreenRenderingViewControllerMock.VerifyNoOtherCalls();

        _sut = null;
        _offscreenRenderingViewControllerMock = null;
    }

    private void VerifyActivate()
    {
        _offscreenRenderingViewControllerMock.Verify(m => m.Activate(), Times.Once);
    }

    [Test]
    public void Activate()
    {
        _sut.Activate();

        VerifyActivate();
    }

    [Test]
    public void Deactivate()
    {
        _sut.Activate();
        _sut.Deactivate();

        VerifyActivate();
        _offscreenRenderingViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
    }
}

}
