using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class DebugRenderingUIControllerTest
{
    private (DebugRenderingUIControler, MockServiceLocator) CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IDebugRenderingUI>();
        serviceLocator.RegisterMock<IRenderTextureView>();

        var sut = new DebugRenderingUIControler(serviceLocator);

        sut.Activate(It.IsAny<IRenderTextureView>());

        // 表示するテクスチャをUIのActivate時に渡す
        serviceLocator.GetMock<IDebugRenderingUI>().Verify(m => m.Activate(It.IsAny<IRenderTextureView>()), Times.Once);

        return (sut, serviceLocator);
    }

    [Test]
    public void Activate()
    {
        var (sut, sl) = CreateSUT();

        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void Deactivate()
    {
        var (sut, sl) = CreateSUT();

        sut.Deactivate();

        sl.GetMock<IDebugRenderingUI>().Verify(m => m.Deactivate(), Times.Once);
        sl.VerifyNoOtherCallsAll();
    }
}

}
