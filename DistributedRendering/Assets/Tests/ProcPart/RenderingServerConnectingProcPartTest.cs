using Moq;
using NUnit.Framework;

public class RenderingServerConnectingProcPartTest
{
    [Test]
    public void StartConnectTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続処理が呼び出されて、接続中の表示になる
        namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        bool isConnected = false;
        bool isFailed = false;
        namedPipeClientMock.Object.OnConnected += () => isConnected = true;
        namedPipeClientMock.Object.OnFailed += () => isFailed = true;

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        // 接続中
        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        // 接続成功
        namedPipeClientMock.Raise(m => m.OnConnected += null);

        Assert.IsTrue(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);
    }

    [Test]
    public void ConnectTimeOutTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();

        var namedPipeClientMock = new Mock<INamedPipeClient>();

        bool isConnected = false;
        bool isFailed = false;
        namedPipeClientMock.Object.OnConnected += () => isConnected = true;
        namedPipeClientMock.Object.OnFailed += () => isFailed = true;

        var timerCreator = new TestTimerCreator();

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                timerCreator);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        // 接続中
        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        // 接続失敗
        namedPipeClientMock.Raise(m => m.OnFailed += null);

        Assert.IsFalse(isConnected);
        Assert.IsTrue(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        // 失敗表示の表示時間を終了させる
        timerCreator.EndTimer(0);

        // Resetメソッドが呼ばれる
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Once);
    }
}
