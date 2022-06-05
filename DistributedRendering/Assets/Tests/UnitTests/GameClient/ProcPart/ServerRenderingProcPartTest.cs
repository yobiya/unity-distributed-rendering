using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace GameClient
{

public class ServerRenderingProcPartTest
{
    private ServerRenderingProcPart _sut;
    private Mock<IRenderingUIController> _renderingUIControllerMock;
    private Mock<ISyncronizeSerializeViewController> _syncronizeSerializeViewControllerMock;
    private Mock<INamedPipeClient> _namedPipeClientMock;

    [SetUp]
    public void SetUp()
    {
        _renderingUIControllerMock = new Mock<IRenderingUIController>();
        _syncronizeSerializeViewControllerMock = new Mock<ISyncronizeSerializeViewController>();
        _namedPipeClientMock = new Mock<INamedPipeClient>();

        _sut
            = new ServerRenderingProcPart(
                _renderingUIControllerMock.Object,
                _syncronizeSerializeViewControllerMock.Object,
                _namedPipeClientMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _renderingUIControllerMock.VerifyNoOtherCalls();
        _syncronizeSerializeViewControllerMock.VerifyNoOtherCalls();
        _namedPipeClientMock.VerifyNoOtherCalls();

        _sut = null;
        _renderingUIControllerMock = null;
        _syncronizeSerializeViewControllerMock = null;
        _namedPipeClientMock = null;
    }

    [UnityTest]
    public IEnumerator ActivateToDeactivate() => UniTask.ToCoroutine(async () =>
    {
        // ActivateAsyncメソッドはDeactivateメソッドが呼ばれるまで終わらない処理なので
        // 最後の処理が呼び出されたら、Deactivateメソッドを呼び出して終了させる
        _renderingUIControllerMock
            .Setup(m => m.RenderImageBuffer(It.IsAny<byte[]>()))
            .Callback(_sut.Deactivate);

        await _sut.ActivateAsync();

        // 表示用のUIControllerはServerRenderingProcPartの有効化と無効化に合わせて、同じく有効化と無効化される
        _renderingUIControllerMock.Verify(m => m.Activate(), Times.Once);
        _renderingUIControllerMock.Verify(m => m.Deactivate(), Times.Once);

        // 同期するデータをシリアライズして、レンダリングサーバーに送る
        _syncronizeSerializeViewControllerMock.Verify(m => m.Serialize(), Times.Once);
        _namedPipeClientMock.Verify(m => m.SendDataAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once);

        // レンダリングサーバーからデータを受け取って、それを表示用に書き込む
        _namedPipeClientMock.Verify(m => m.RecieveDataAsync(It.IsAny<CancellationToken>()), Times.Once);
        _renderingUIControllerMock.Verify(m => m.RenderImageBuffer(It.IsAny<byte[]>()), Times.Once);
    });
}

}
