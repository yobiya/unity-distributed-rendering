using System;
using Moq;
using NUnit.Framework;

public class RenderingServerConnectingProcPartTest
{
    struct TestCollection
    {
        public RenderingServerConnectingProcPart sut;
        public Mock<IRenderingServerConnectingUIViewController> renderingServerConnectingUIViewControllerMock;
        public Mock<ITestMessageSendUIViewController> testMessageSendUIViewControllerMock;
        public Mock<INamedPipeClient> namedPipeClientMock;
        public TestTimerCreator timerCreator;

        public void MockVerifyNoOtherCalls()
        {
            renderingServerConnectingUIViewControllerMock.VerifyNoOtherCalls();
            testMessageSendUIViewControllerMock.VerifyNoOtherCalls();
            namedPipeClientMock.VerifyNoOtherCalls();
        }
    }

    private TestCollection CreateSUT()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIViewController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();
        var timerCreator = new TestTimerCreator();

        // イベントのVerifyができるように設定する
        {
            renderingServerConnectingUIViewControllerMock.SetupAdd(m => m.OnRequestConnecting += It.IsAny<Action>());
            testMessageSendUIViewControllerMock.SetupAdd(m => m.OnSend += It.IsAny<Action>());
            namedPipeClientMock.SetupAdd(m => m.OnConnected += It.IsAny<Action>()).Verifiable();
            namedPipeClientMock.SetupAdd(m => m.OnFailed += It.IsAny<Action>()).Verifiable();
        }

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                namedPipeClientMock.Object,
                timerCreator);

        // コンストラクタで登録されたイベントのVerifyを行う
        {
            renderingServerConnectingUIViewControllerMock.VerifyAdd(m => m.OnRequestConnecting += It.IsAny<Action>(), Times.Once);
            testMessageSendUIViewControllerMock.VerifyAdd(m => m.OnSend += It.IsAny<Action>());
            namedPipeClientMock.VerifyAdd(m => m.OnConnected += It.IsAny<Action>(), Times.Once);
            namedPipeClientMock.VerifyAdd(m => m.OnFailed += It.IsAny<Action>(), Times.Once);
        }

        var collection = new TestCollection();
        collection.sut = sut;
        collection.renderingServerConnectingUIViewControllerMock = renderingServerConnectingUIViewControllerMock;
        collection.testMessageSendUIViewControllerMock = testMessageSendUIViewControllerMock;
        collection.namedPipeClientMock = namedPipeClientMock;
        collection.timerCreator = timerCreator;

        return collection;
    }

    [Test]
    public void Activate()
    {
        var collection = CreateSUT();

        // 初期状態は有効になっているので、一度無効してからActivateを呼び出す
        collection.sut.Deactivate();
        collection.sut.Activate();

        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.Deactivate());
        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.Activate());
        collection.MockVerifyNoOtherCalls();
    }

    [Test]
    public void Deactivate()
    {
        var collection = CreateSUT();

        collection.sut.Deactivate();

        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.Deactivate());
        collection.MockVerifyNoOtherCalls();
    }

    [Test]
    public void StartConnectTest()
    {
        var collection = CreateSUT();

        // UIから接続のリクエストが呼ばれる
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続処理が呼び出されて、接続中の表示になる
        {
            collection.namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()), Times.Once);
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        }

        collection.MockVerifyNoOtherCalls();
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var collection = CreateSUT();

        // UIから接続のリクエストが呼ばれる
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);
        collection.namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()));

        // 接続成功
        collection.namedPipeClientMock.Raise(m => m.OnConnected += null);

        {
            // UIが接続済みの表示になる
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Once);

            // テストメッセージ送信用のボタンが表示になる
            collection.testMessageSendUIViewControllerMock.Verify(m => m.Activate(), Times.Once);
        }

        collection.MockVerifyNoOtherCalls();
    }

    [Test]
    public void ConnectTimeOutTest()
    {
        var collection = CreateSUT();

        // UIから接続のリクエストが呼ばれる
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        // 接続失敗
        collection.namedPipeClientMock.Raise(m => m.OnFailed += null);

        // 接続に失敗したら、UIが接続失敗の表示になる
        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);

        // 失敗表示の表示時間を終了させる
        collection.timerCreator.EndTimer(0);

        // 接続失敗の表示時間が終了したら、Resetが呼ばれてUIが初期状態に戻る
        {
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);
            collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.Reset(), Times.Once);
        }
    }

    [Test]
    public void SendTestMessage()
    {
        var collection = CreateSUT();

        // UIから接続のリクエストが呼ばれる
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);
        collection.namedPipeClientMock.Verify(m => m.Connect(It.IsAny<int>()));

        // 接続成功
        collection.namedPipeClientMock.Raise(m => m.OnConnected += null);

        // テストメッセージを送信
        collection.testMessageSendUIViewControllerMock.Raise(m => m.OnSend += null);

        collection.namedPipeClientMock.Verify(m => m.Write("Test message."), Times.Once);
    }
}