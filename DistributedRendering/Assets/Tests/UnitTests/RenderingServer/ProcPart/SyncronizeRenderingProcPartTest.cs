using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RenderingServer
{

public class SyncronizeRenderingProcPartTest
{
    private SyncronizeRenderingProcPart _sut;
    private Mock<INamedPipeServer> _namedPipeServerMock;
    private Mock<IResponseDataNamedPipe> _responseDataNamedPipeMock;
    private Mock<ISyncCameraViewController> _syncCameraViewControllerMock;
    private Mock<IOffscreenRenderingViewController> _offscreenRenderingViewControllerMock;
    private Mock<ISyncronizeDeserializeViewController> _syncronizeDeserializeViewController;
    private Mock<IDebugRenderingUIControler> _debugRenderingUIControlerMock;

    [SetUp]
    public void SetUp()
    {
        _namedPipeServerMock = new Mock<INamedPipeServer>();
        _responseDataNamedPipeMock = new Mock<IResponseDataNamedPipe>();
        _syncCameraViewControllerMock = new Mock<ISyncCameraViewController>();
        _offscreenRenderingViewControllerMock = new Mock<IOffscreenRenderingViewController>();
        _syncronizeDeserializeViewController = new Mock<ISyncronizeDeserializeViewController>();
        _debugRenderingUIControlerMock = new Mock<IDebugRenderingUIControler>();
        _sut = new SyncronizeRenderingProcPart(
            _namedPipeServerMock.Object,
            _responseDataNamedPipeMock.Object,
            _syncronizeDeserializeViewController.Object,
            _syncCameraViewControllerMock.Object,
            _offscreenRenderingViewControllerMock.Object,
            _debugRenderingUIControlerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _namedPipeServerMock.VerifyNoOtherCalls();
        _responseDataNamedPipeMock.VerifyNoOtherCalls();
        _syncronizeDeserializeViewController.VerifyNoOtherCalls();
        _syncCameraViewControllerMock.VerifyNoOtherCalls();
        _offscreenRenderingViewControllerMock.VerifyNoOtherCalls();
        _debugRenderingUIControlerMock.VerifyNoOtherCalls();

        _sut = null;
        _namedPipeServerMock = null;
        _responseDataNamedPipeMock = null;
        _syncronizeDeserializeViewController = null;
        _syncCameraViewControllerMock = null;
        _offscreenRenderingViewControllerMock = null;
        _debugRenderingUIControlerMock = null;
    }

    [UnityTest]
    public IEnumerator ActivateToDeactivate() => UniTask.ToCoroutine(async () =>
    {
        // ActivateAsyncはDesactivateが呼ばれるまで終わらないので
        // 最後のメソッドが呼ばれたときにDeactivateを読んで終了させる
        _responseDataNamedPipeMock
            .Setup(m => m.SendRenderingImage(It.IsAny<RenderTexture>()))
            .Callback(_sut.Deactivate);

        await _sut.ActivateAsync();

        // SyncronizeRenderingProcPartの有効化と無効化時に、一緒に有効化と無効化される要素
        _syncCameraViewControllerMock.Verify(m => m.Activate(), Times.Once);
        _syncCameraViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        _offscreenRenderingViewControllerMock.Verify(m => m.Activate(), Times.Once);
        _offscreenRenderingViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.Activate(It.IsAny<RenderTexture>()), Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.Deactivate(), Times.Once);

        // レンダリングした画像をデバッグ表示とゲームクライアント送る場合で２回呼ばれる
        _offscreenRenderingViewControllerMock.VerifyGet(m => m.RenderTexture, Times.Exactly(2));

        // ゲームクライアントからデータを受け取って同期させる
        _responseDataNamedPipeMock.Verify(m => m.RecieveDataAsync(It.IsAny<CancellationToken>()), Times.Once);
        _syncronizeDeserializeViewController.Verify(m => m.Deserialize(It.IsAny<byte[]>()), Times.Once);

        // 同期した後にレンダリングした画像をゲームクライアントに送る
        _responseDataNamedPipeMock.Verify(m => m.SendRenderingImage(It.IsAny<RenderTexture>()), Times.Once);

        // NamedPipeServerはゲームクライアントと接続済みの状態で渡されるのでActivateは呼ばれないが
        // SyncronizeRenderingProcPart.Deactivateが呼ばれたときに接続を終了するので、Deactivateは呼ばれる
        _namedPipeServerMock.Verify(m => m.Deactivate(), Times.Once);
    });
}

}
