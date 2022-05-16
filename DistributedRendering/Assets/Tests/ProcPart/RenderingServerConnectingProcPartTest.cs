using System;
using Moq;
using NUnit.Framework;

public class RenderingServerConnectingProcPartTest
{
    public interface ITestRenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
    {
        // 接続リクエストボタンが押された場合の処理を呼び出す
        void RequestConnect();
    }

    class TestRenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
    {
        // テスト用変数
        public bool isCalledShowConnecting = false;
        public bool isCalledShowConnected = false;
        public bool isCalledShowFailed = false;
        public bool isCalledReset = false;

        public event Action OnRequestConnecting;

        public void ShowConnecting() => isCalledShowConnecting = true;
        public void ShowConnected() => isCalledShowConnected = true;
        public void ShowFailed() => isCalledShowFailed = true;
        public void Reset() => isCalledReset = true;

        // テスト用メソッド
        public void RequestConnect() => OnRequestConnecting?.Invoke();
    }

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
        var renderingServerConnectingUIViewControllerMock = new Mock<ITestRenderingServerConnectingUIViewController>();
        renderingServerConnectingUIViewControllerMock
            .Setup(m => m.RequestConnect()).Callback(() => 
                renderingServerConnectingUIViewControllerMock
                    .Raise(x => x.OnRequestConnecting += null));

        var namedPipeClientMock = new Mock<INamedPipeClient>();

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        renderingServerConnectingUIViewControllerMock.Object.RequestConnect();
        namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void ConnectingTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient, new TestTimerCreator());

        renderingServerConnectingUIViewController.RequestConnect();

        Assert.IsFalse(isConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient, new TestTimerCreator());

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);

        renderingServerConnectingUIViewController.RequestConnect();

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);

        namedPipeClient.SuccessConnect();

        // 接続成功
        Assert.IsTrue(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);
    }

    [Test]
    public void ConnectTimeOutTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var timerCreator = new TestTimerCreator();

        var procPart
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewController,
                namedPipeClient,
                timerCreator);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);

        renderingServerConnectingUIViewController.RequestConnect();

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);

        namedPipeClient.FailConnect();

        // 接続失敗
        Assert.IsFalse(isConnected);
        Assert.IsTrue(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledReset);

        // 失敗表示の表示時間を終了させる
        timerCreator.EndTimer(0);

        // Resetメソッドが呼ばれる
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledReset);
    }
}
