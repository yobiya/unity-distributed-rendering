using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class OffscreenRenderingProcPartTest
{
    private (OffscreenRenderingProcPart, MockServiceLocator) CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IOffscreenRenderingViewController>();

        var sut = new OffscreenRenderingProcPart(serviceLocator);

        // 初期状態は無効になっているので、有効化する
        sut.Activate();

        // CreateSUTメソッド内でsut.Activateメソッドが呼ばれているので
        // IOffscreenRenderingViewController.Activateメソッドも呼ばれる
        serviceLocator.GetMock<IOffscreenRenderingViewController>().Verify(m => m.Activate(), Times.Once);

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

        // sutが無効化された場合に、IOffscreenRenderingViewControllerも無効化される
        serviceLocator.GetMock<IOffscreenRenderingViewController>().Verify(m => m.Deactivate(), Times.Once);

        serviceLocator.VerifyNoOtherCallsAll();
    }
}

}
