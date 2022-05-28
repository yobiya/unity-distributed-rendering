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

        var sut = new DebugRenderingUIControler(serviceLocator);

        sut.Activate();

        serviceLocator.GetMock<IDebugRenderingUI>().Verify(m => m.Activate(), Times.Once);

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
