using Common;
using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class OffscreenRenderingProcPartTest
{
    private struct TestCollection
    {
        public OffscreenRenderingProcPart sut;
        public MockServiceLocator serviceLocator;
    }

    private TestCollection CreateSUT()
    {
        var serviceLocator = new MockServiceLocator();
        serviceLocator.RegisterMock<IOffscreenRenderingViewController>();

        var sut = new OffscreenRenderingProcPart(serviceLocator);

        // 初期状態は無効になっているので、有効化する
        sut.Activate();

        // CreateSUTメソッド内でsut.Activateメソッドが呼ばれているので
        // IOffscreenRenderingViewController.Activateメソッドも呼ばれる
        serviceLocator.GetMock<IOffscreenRenderingViewController>().Verify(m => m.Activate(), Times.Once);

        var collection = new TestCollection();
        collection.sut = sut;
        collection.serviceLocator = serviceLocator;

        return collection;
    }

    [Test]
    public void Activate()
    {
        var collection = CreateSUT();

        collection.serviceLocator.GetMock<IOffscreenRenderingViewController>().VerifyNoOtherCalls();
    }

    [Test]
    public void Deactivate()
    {
        var collection = CreateSUT();

        collection.sut.Deactivate();

        // sutが無効化された場合に、IOffscreenRenderingViewControllerも無効化される
        collection.serviceLocator.GetMock<IOffscreenRenderingViewController>().Verify(m => m.Deactivate(), Times.Once);

        collection.serviceLocator.GetMock<IOffscreenRenderingViewController>().VerifyNoOtherCalls();
    }
}

}
