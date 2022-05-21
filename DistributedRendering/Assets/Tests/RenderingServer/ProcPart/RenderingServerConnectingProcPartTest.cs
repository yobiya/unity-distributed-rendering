using Moq;
using NUnit.Framework;

public class RenderingServerConnectingProcPartTest
{
    [Test]
    public void Activate()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        // 初期状態は有効になっているので、一度無効してからActivateを呼び出す
        sut.Deactivate();
        sut.Activate();

        renderingServerConnectingUIViewControllerMock.Verify(m => m.Activate());
    }

    [Test]
    public void Deactivate()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        sut.Deactivate();

        renderingServerConnectingUIViewControllerMock.Verify(m => m.Deactivate());
    }

    [Test]
    public void StartConnectTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        bool isConnected = false;
        bool isFailed = false;
        namedPipeClientMock.Object.OnConnected += () => isConnected = true;
        namedPipeClientMock.Object.OnFailed += () => isFailed = true;

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        // 接続前は何も呼ばれない
        {
            Assert.IsFalse(isConnected);
            Assert.IsFalse(isFailed);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);
        }

        // UIから接続のリクエストが呼ばれる
        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続処理が呼び出されて、接続中の表示になる
        {
            Assert.IsFalse(isConnected);
            Assert.IsFalse(isFailed);
            namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);
        }
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        bool isConnected = false;
        bool isFailed = false;
        namedPipeClientMock.Object.OnConnected += () => isConnected = true;
        namedPipeClientMock.Object.OnFailed += () => isFailed = true;

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                new TestTimerCreator());

        // UIから接続のリクエストが呼ばれる
        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続成功
        namedPipeClientMock.Raise(m => m.OnConnected += null);

        {
            Assert.IsTrue(isConnected);
            Assert.IsFalse(isFailed);

            // UIが接続済みの表示になる
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);

            // テストメッセージ送信用のボタンが表示になる
            testMessageSendUIViewControllerMock.Verify(m => m.Activate(), Times.Once);
        }
    }

    [Test]
    public void ConnectTimeOutTest()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();

        bool isConnected = false;
        bool isFailed = false;
        namedPipeClientMock.Object.OnConnected += () => isConnected = true;
        namedPipeClientMock.Object.OnFailed += () => isFailed = true;

        var timerCreator = new TestTimerCreator();

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                timerCreator);

        // UIから接続のリクエストが呼ばれる
        renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続失敗
        namedPipeClientMock.Raise(m => m.OnFailed += null);

        // 接続に失敗したら、UIが接続失敗の表示になる
        {
            Assert.IsFalse(isConnected);
            Assert.IsTrue(isFailed);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Never);
        }

        // 失敗表示の表示時間を終了させる
        timerCreator.EndTimer(0);

        // 接続失敗の表示時間が終了したら、Resetが呼ばれてUIが初期状態に戻る
        {
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Never);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Once);
        }
    }
}
