using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace GameClient
{

public class RenderingServerConnectingProcPartTest
{
    private RenderingServerConnectingProcPart _sut;
    private Mock<IRenderingServerConnectingUIController> _renderingServerConnectingUIControllerMock;
    private Mock<ITestMessageSendUIViewController> _testMessageSendUIViewControllerMock;
    private Mock<INamedPipeClient> _namedPipeClientMock;
    private Mock<ITimerCreator> _timerCreatorMock;

    [SetUp]
    public void SetUp()
    {
        _renderingServerConnectingUIControllerMock = new Mock<IRenderingServerConnectingUIController>();
        _testMessageSendUIViewControllerMock = new Mock<ITestMessageSendUIViewController>();
        _namedPipeClientMock = new Mock<INamedPipeClient>();
        _timerCreatorMock = new Mock<ITimerCreator>();

        _renderingServerConnectingUIControllerMock.SetupAdd(m => m.OnRequestConnecting += It.IsAny<Action>());
        _testMessageSendUIViewControllerMock.SetupAdd(m => m.OnSend += It.IsAny<Action>());

        _sut
            = new RenderingServerConnectingProcPart(
                _renderingServerConnectingUIControllerMock.Object,
                _testMessageSendUIViewControllerMock.Object,
                _namedPipeClientMock.Object,
                _timerCreatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _renderingServerConnectingUIControllerMock.VerifyNoOtherCalls();
        _testMessageSendUIViewControllerMock.VerifyNoOtherCalls();
        _namedPipeClientMock.VerifyNoOtherCalls();
        _timerCreatorMock.VerifyNoOtherCalls();

        _sut = null;
        _renderingServerConnectingUIControllerMock = null;
        _testMessageSendUIViewControllerMock = null;
        _namedPipeClientMock = null;
        _timerCreatorMock = null;
    }

    private void VerifyActivate()
    {
        _renderingServerConnectingUIControllerMock.VerifyAdd(m => m.OnRequestConnecting += It.IsAny<Action>(), Times.Once);
        _renderingServerConnectingUIControllerMock.Verify(m => m.Activate(), Times.Once);
        _testMessageSendUIViewControllerMock.VerifyAdd(m => m.OnSend += It.IsAny<Action>());
        _renderingServerConnectingUIControllerMock.Verify(m => m.ShowConnecting(), Times.Once);
        _namedPipeClientMock.Verify(m => m.ConnectAsync(It.IsAny<int>()), Times.Once);
    }

    [UnityTest]
    public IEnumerator ActivateAndConncted() => UniTask.ToCoroutine(async () =>
    {
        _namedPipeClientMock
            .Setup(m => m.ConnectAsync(It.IsAny<int>()))
            .Returns(UniTask.FromResult<INamedPipeClient.ConnectResult>(INamedPipeClient.ConnectResult.Connected));

        var result = INamedPipeClient.ConnectResult.TimeOut;
        await UniTask.WhenAll(
            UniTask.Defer(async () => result = await _sut.Activate()),
            UniTask.Defer(async () =>
            {
                await UniTask.NextFrame();
                _renderingServerConnectingUIControllerMock.Raise(m => m.OnRequestConnecting += null);
            }));

        Assert.AreEqual(INamedPipeClient.ConnectResult.Connected, result);
        VerifyActivate();

        _renderingServerConnectingUIControllerMock.Verify(m => m.ShowConnected(), Times.Once);
        _testMessageSendUIViewControllerMock.Verify(m => m.Activate(), Times.Once);
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
        _namedPipeClientMock
            .Setup(m => m.ConnectAsync(It.IsAny<int>()))
            .Returns(UniTask.FromResult<INamedPipeClient.ConnectResult>(INamedPipeClient.ConnectResult.TimeOut));

        _timerCreatorMock
            .Setup(m => m.Create(It.IsAny<float>()))
            .Returns(UniTask.CompletedTask);

        var result = INamedPipeClient.ConnectResult.Connected;
        await UniTask.WhenAll(
            UniTask.Defer(async () => result = await _sut.Activate()),
            UniTask.Defer(async () =>
            {
                await UniTask.NextFrame();
                _renderingServerConnectingUIControllerMock.Raise(m => m.OnRequestConnecting += null);
            }));

        Assert.AreEqual(INamedPipeClient.ConnectResult.TimeOut, result);
        VerifyActivate();

        _renderingServerConnectingUIControllerMock.Verify(m => m.ShowFailed(), Times.Once);
        _timerCreatorMock.Verify(m => m.Create(It.IsAny<float>()), Times.Once);
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
