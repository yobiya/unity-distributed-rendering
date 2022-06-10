using System.Collections;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using MessagePackFormat;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameClient
{

public class ServerRenderingProcPartTest
{
    private ServerRenderingProcPart _sut;
    private Mock<IRenderingUIController> _renderingUIControllerMock;
    private Mock<ISyncronizeSerializeViewController> _syncronizeSerializeViewControllerMock;
    private Mock<INamedPipeClient> _namedPipeClientMock;
    private Mock<ISerializer> _serializerMock;

    [SetUp]
    public void SetUp()
    {
        _renderingUIControllerMock = new Mock<IRenderingUIController>();
        _syncronizeSerializeViewControllerMock = new Mock<ISyncronizeSerializeViewController>();
        _namedPipeClientMock = new Mock<INamedPipeClient>();
        _serializerMock = new Mock<ISerializer>();;

        _sut
            = new ServerRenderingProcPart(
                _renderingUIControllerMock.Object,
                _syncronizeSerializeViewControllerMock.Object,
                _namedPipeClientMock.Object,
                _serializerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _renderingUIControllerMock.VerifyNoOtherCalls();
        _syncronizeSerializeViewControllerMock.VerifyNoOtherCalls();
        _namedPipeClientMock.VerifyNoOtherCalls();
        _serializerMock.VerifyNoOtherCalls();

        _sut = null;
        _renderingUIControllerMock = null;
        _syncronizeSerializeViewControllerMock = null;
        _namedPipeClientMock = null;
        _serializerMock = null;
    }

    [UnityTest]
    public IEnumerator ActivateToDeactivate() => UniTask.ToCoroutine(async () =>
    {
        // ActivateAsyncメソッドはDeactivateメソッドが呼ばれるまで終わらない処理なので
        // 最後の処理が呼び出されたら、Deactivateメソッドを呼び出して終了させる
        _renderingUIControllerMock
            .Setup(m => m.MargeImage(It.IsAny<byte[]>()))
            .Callback(_sut.Deactivate);

        _serializerMock
            .Setup(m => m.Serialize(It.IsAny<SetupData>()))
            .Returns(() =>
                {
                    var data = new byte[2];
                    data[0] = (byte)NamedPipeDefinisions.Command.Setup; // 先頭にコマンドの値
                    data[1] = 1;                                        // ダミーの本体
                    return data;
                });

        await _sut.ActivateAsync(It.IsAny<Camera>());

        // 表示用のUIControllerはServerRenderingProcPartの有効化と無効化に合わせて、同じく有効化と無効化される
        _renderingUIControllerMock.Verify(m => m.Activate(It.IsAny<Camera>(), It.IsAny<SetupData>()), Times.Once);
        _renderingUIControllerMock.Verify(m => m.Deactivate(), Times.Once);

        // 描画設定をシリアライズ
        _serializerMock.Verify(m => m.Serialize(It.IsAny<SetupData>()), Times.Once);

        // 同期するデータをシリアライズ
        _syncronizeSerializeViewControllerMock.Verify(m => m.Serialize(), Times.Once);

        // 描画設定と同期するデータで2回送信する
        _namedPipeClientMock.Verify(m => m.SendDataAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Exactly(2));

        // 自分の担当する範囲の描画を行う
        _renderingUIControllerMock.Verify(m => m.RenderBaseImage(), Times.Once);

        // レンダリングサーバーからデータを受け取って、それを表示用に書き込む
        _namedPipeClientMock.Verify(m => m.RecieveDataAsync(It.IsAny<CancellationToken>()), Times.Once);
        _renderingUIControllerMock.Verify(m => m.MargeImage(It.IsAny<byte[]>()), Times.Once);
    });
}

}
