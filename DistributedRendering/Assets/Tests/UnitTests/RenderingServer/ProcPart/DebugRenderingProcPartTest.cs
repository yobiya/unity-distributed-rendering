using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class DebugRenderingProcPartTest
{
    private (DebugRenderingProcPart, MockServiceLocator) CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IDebugRenderingUIControler>();

        var sut = new DebugRenderingProcPart(serviceLocator);

        sut.Activate(null);

        serviceLocator.GetMock<IDebugRenderingUIControler>().Verify(m => m.Activate(), Times.Once);

        return (sut, serviceLocator);
    }

    [Test]
    public void Activate()
    {
        var (sut, serviceLocator) = CreateSUT();

        serviceLocator.VerifyNoOtherCallsAll();
    }

    [Test]
    public void Deactivate()
    {
        var (sut, serviceLocator) = CreateSUT();

        sut.Deactivate();

        serviceLocator.GetMock<IDebugRenderingUIControler>().Verify(m => m.Deactivate(), Times.Once);
        serviceLocator.VerifyNoOtherCallsAll();
    }
}

}
