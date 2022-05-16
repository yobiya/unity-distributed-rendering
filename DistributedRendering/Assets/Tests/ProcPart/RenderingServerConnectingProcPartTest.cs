using System;
using Moq;
using NUnit.Framework;

public class RenderingServerConnectingProcPartTest
{
    class TestNamedPipeClient : INamedPipeClient
    {
        public bool isCalledConnect = false;

        public event Action OnConnected;
        public event Action OnFailed;

        public void Connect(int timeOutTime) => isCalledConnect = true;
        public void Write(string text) {}

        // テスト用メソッド
        public void SuccessConnect() => OnConnected?.Invoke();
        public void FailConnect() => OnFailed?.Invoke();
    }

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

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClient,
                new TestTimerCreator());

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        namedPipeClient.SuccessConnect();

        // 接続成功
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

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var timerCreator = new TestTimerCreator();

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClient,
                timerCreator);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
        renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

        namedPipeClient.FailConnect();

        // 接続失敗
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
