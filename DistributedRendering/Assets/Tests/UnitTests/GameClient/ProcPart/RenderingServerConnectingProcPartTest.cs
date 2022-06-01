using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Moq;
using UnityEngine.TestTools;

namespace GameClient
{

public class RenderingServerConnectingProcPartTest
{
    struct TestCollection
    {
        public RenderingServerConnectingProcPart sut;
        public Mock<IRenderingServerConnectingUIController> renderingServerConnectingUIViewControllerMock;
        public Mock<ITestMessageSendUIViewController> testMessageSendUIViewControllerMock;
        public Mock<ICameraViewController> cameraViewControllerMock;
        public Mock<INamedPipeClient> namedPipeClientMock;
        public TestTimerCreator timerCreator;

        public void MockVerifyConnected()
        {
            namedPipeClientMock.Verify(m => m.ConnectAsync(It.IsAny<int>()), Times.Once);
            renderingServerConnectingUIViewControllerMock.VerifyAdd(m => m.OnRequestConnecting += It.IsAny<Action>(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
            renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnected(), Times.Once);
            testMessageSendUIViewControllerMock.Verify(m => m.Activate(), Times.Once);
        }

        public void MockVerifyNoOtherCalls()
        {
            renderingServerConnectingUIViewControllerMock.VerifyNoOtherCalls();
            testMessageSendUIViewControllerMock.VerifyNoOtherCalls();
            namedPipeClientMock.VerifyNoOtherCalls();
        }
    }

    private (TestCollection, UniTask) CreateSUT()
    {
        var renderingServerConnectingUIViewControllerMock = new Mock<IRenderingServerConnectingUIController>();
        var testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        var cameraViewControllerMock = new Mock<ICameraViewController>();
        var namedPipeClientMock = new Mock<INamedPipeClient>();
        var timerCreator = new TestTimerCreator();

        // イベントのVerifyができるように設定する
        {
            renderingServerConnectingUIViewControllerMock.SetupAdd(m => m.OnRequestConnecting += It.IsAny<Action>());
            testMessageSendUIViewControllerMock.SetupAdd(m => m.OnSend += It.IsAny<Action>());
            namedPipeClientMock.SetupAdd(m => m.OnRecieved += It.IsAny<Action<byte[]>>());
        }

        var sut
            = new RenderingServerConnectingProcPart(
                renderingServerConnectingUIViewControllerMock.Object,
                testMessageSendUIViewControllerMock.Object,
                cameraViewControllerMock.Object,
                namedPipeClientMock.Object,
                timerCreator);

        var task = sut.Activate();

        // コンストラクタで登録されたイベントのVerifyを行う
        {
            renderingServerConnectingUIViewControllerMock.Verify(m => m.Activate(), Times.Once);
            testMessageSendUIViewControllerMock.VerifyAdd(m => m.OnSend += It.IsAny<Action>());
            namedPipeClientMock.VerifyAdd(m => m.OnRecieved += It.IsAny<Action<byte[]>>(), Times.Once);
        }

        var collection = new TestCollection();
        collection.sut = sut;
        collection.renderingServerConnectingUIViewControllerMock = renderingServerConnectingUIViewControllerMock;
        collection.testMessageSendUIViewControllerMock = testMessageSendUIViewControllerMock;
        collection.cameraViewControllerMock = cameraViewControllerMock;
        collection.namedPipeClientMock = namedPipeClientMock;
        collection.timerCreator = timerCreator;

        return (collection, task);
    }

    private async UniTask CreateConnectedTask(TestCollection collection)
    {
        await UniTask.NextFrame();
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        await UniTask.NextFrame();
        collection
            .namedPipeClientMock
                .Setup(m => m.ConnectAsync(It.IsAny<int>()))
                .Returns(UniTask.FromResult<INamedPipeClient.ConnectResult>(INamedPipeClient.ConnectResult.Connected));
    }

    private async UniTask CreateTimeOutTask(TestCollection collection)
    {
        await UniTask.NextFrame();
        collection.renderingServerConnectingUIViewControllerMock.Raise(m => m.OnRequestConnecting += null);

        await UniTask.NextFrame();

        // 接続失敗
        await UniTask.NextFrame();

        // 失敗表示の表示時間を終了させる
        collection.timerCreator.EndTimer(0);
    }

    [UnityTest]
    public IEnumerator ActivateAndConncted() => UniTask.ToCoroutine(async () =>
    {
        var (collection, task) = CreateSUT();

        await UniTask.WhenAll(
            task,
            CreateConnectedTask(collection));

        collection.MockVerifyConnected();
        collection.MockVerifyNoOtherCalls();
    });
/*
    [Test]
    public IEnumerator Deactivate() => UniTask.ToCoroutine(async () =>
    {
        var (collection, task) = CreateSUT();

        await task;

        collection.sut.Deactivate();

        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        collection.MockVerifyNoOtherCalls();
    });
*/
    [UnityTest]
    public IEnumerator ActivateAndConnectTimeOutTest() => UniTask.ToCoroutine(async () =>
    {
        var (collection, task) = CreateSUT();

        collection
            .namedPipeClientMock
                .Setup(m => m.ConnectAsync(It.IsAny<int>()))
                .Returns(UniTask.FromResult<INamedPipeClient.ConnectResult>(INamedPipeClient.ConnectResult.TimeOut));

        await UniTask.WhenAll(
            task,
            CreateTimeOutTask(collection));

        collection.renderingServerConnectingUIViewControllerMock.VerifyAdd(m => m.OnRequestConnecting += It.IsAny<Action>(), Times.Once);
        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        collection.namedPipeClientMock.Verify(m => m.ConnectAsync(It.IsAny<int>()), Times.Once);
        collection.renderingServerConnectingUIViewControllerMock.Verify(m => m.ShowFailed(), Times.Once);

        collection.MockVerifyNoOtherCalls();
    });
/*
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
    }*/
}

}
