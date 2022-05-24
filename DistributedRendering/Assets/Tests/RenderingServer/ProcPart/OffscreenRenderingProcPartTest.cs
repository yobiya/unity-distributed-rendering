using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class OffscreenRenderingProcPartTest
{
    private struct TestCollection
    {
        public OffscreenRenderingProcPart sut;
        public Mock<IOffscreenRenderingViewController> offscreenRenderingViewControllerMock;
    }

    private TestCollection CreateSUT()
    {
        var offscreenRenderingViewControllerMock = new Mock<IOffscreenRenderingViewController>();
        var sut = new OffscreenRenderingProcPart(offscreenRenderingViewControllerMock.Object);

        // 初期状態は無効になっているので、有効化する
        sut.Activate();

        var collection = new TestCollection();
        collection.sut = sut;
        collection.offscreenRenderingViewControllerMock = offscreenRenderingViewControllerMock;

        return collection;
    }

    [Test]
    public void Activate()
    {
        var collection = CreateSUT();

        // CreateSUTメソッド内でsut.Activateメソッドが呼ばれているので
        // IOffscreenRenderingViewController.Activateメソッドも呼ばれる
        collection.offscreenRenderingViewControllerMock.Verify(m => m.Activate(), Times.Once);

        collection.offscreenRenderingViewControllerMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Deactivate()
    {
        var collection = CreateSUT();

        collection.sut.Deactivate();

        // sutが無効化された場合に、IOffscreenRenderingViewControllerも無効化される
        collection.offscreenRenderingViewControllerMock.Verify(m => m.Deactivate(), Times.Once);

        collection.offscreenRenderingViewControllerMock.Verify(m => m.Activate(), Times.Once);
        collection.offscreenRenderingViewControllerMock.VerifyNoOtherCalls();
    }
}

}
