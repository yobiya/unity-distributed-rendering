using System;
using System.Buffers;
using System.Collections;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using MessagePackFormat;
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
    private Mock<ISerializer> _serializerMock;
    private Mock<IOffscreenRenderingViewController> _offscreenRenderingViewControllerMock;
    private Mock<ISyncronizeDeserializeViewController> _syncronizeDeserializeViewController;
    private Mock<IDebugRenderingUIControler> _debugRenderingUIControlerMock;

    [SetUp]
    public void SetUp()
    {
        _namedPipeServerMock = new Mock<INamedPipeServer>();
        _serializerMock = new Mock<ISerializer>();
        _offscreenRenderingViewControllerMock = new Mock<IOffscreenRenderingViewController>();
        _syncronizeDeserializeViewController = new Mock<ISyncronizeDeserializeViewController>();
        _debugRenderingUIControlerMock = new Mock<IDebugRenderingUIControler>();
        _sut = new SyncronizeRenderingProcPart(
            _namedPipeServerMock.Object,
            _serializerMock.Object,
            _syncronizeDeserializeViewController.Object,
            _offscreenRenderingViewControllerMock.Object,
            _debugRenderingUIControlerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _namedPipeServerMock.VerifyNoOtherCalls();
        _serializerMock.VerifyNoOtherCalls();
        _syncronizeDeserializeViewController.VerifyNoOtherCalls();
        _offscreenRenderingViewControllerMock.VerifyNoOtherCalls();
        _debugRenderingUIControlerMock.VerifyNoOtherCalls();

        _sut = null;
        _namedPipeServerMock = null;
        _serializerMock = null;
        _syncronizeDeserializeViewController = null;
        _offscreenRenderingViewControllerMock = null;
        _debugRenderingUIControlerMock = null;
    }

    [UnityTest]
    public IEnumerator ActivateToDeactivate() => UniTask.ToCoroutine(async () =>
    {
        _namedPipeServerMock
            .SetupSequence(m => m.RecieveDataAsync(It.IsAny<CancellationToken>()))
            .Returns(() =>
                {
                    // 1回目は設定データを返す
                    var data = new byte[2];
                    data[0] = (byte)NamedPipeDefinisions.Command.Setup; // 先頭にコマンドの値
                    data[1] = 1;                                        // ダミーの本体
                    return UniTask.FromResult(data);
                })
            .Returns(() =>
                {
                    // 2回目は同期データを返す
                    var data = new byte[2];
                    data[0] = (byte)NamedPipeDefinisions.Command.Syncronize;
                    data[1] = 1;
                    return UniTask.FromResult(data);
                });

        _serializerMock
            .Setup(m => m.Deserialize<SetupData>(It.IsAny<ReadOnlyMemory<byte>>()))
            .Returns(() => new SetupData());

        // ActivateAsyncはDesactivateが呼ばれるまで終わらないので
        // 最後のメソッドが呼ばれたときにDeactivateを呼んで終了させる
        _namedPipeServerMock
            .Setup(m => m.SendDataAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .Callback(_sut.Deactivate);

        await _sut.ActivateAsync();

        // SyncronizeRenderingProcPartの有効化と無効化時に、一緒に有効化と無効化される要素
        _offscreenRenderingViewControllerMock.Verify(m => m.ActivateAsync(It.IsAny<SetupData>()), Times.Once);
        _offscreenRenderingViewControllerMock.Verify(m => m.Deactivate(), Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.ActivateAsync(), Times.Once);
        _debugRenderingUIControlerMock.Verify(m => m.Deactivate(), Times.Once);

        // 設定データと同期データの2回受信する
        _namedPipeServerMock.Verify(m => m.RecieveDataAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));

        // 受け取った設定データをデシリアライズする
        _serializerMock.Verify(m => m.Deserialize<SetupData>(It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);

        // 受け取った同期データをデシリアライズする
        _syncronizeDeserializeViewController.Verify(m => m.Deserialize(It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);

        // 同期した後にレンダリングした画像をゲームクライアントに送る
        _offscreenRenderingViewControllerMock.Verify(m => m.Render(), Times.Once);
        _namedPipeServerMock.Verify(m => m.SendDataAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once);
    });
}

}
