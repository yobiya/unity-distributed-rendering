using Moq;
using NUnit.Framework;

public class ProcPartBinderTest
{
    [Test]
    public void BindRenderingServerConnectingActive()
    {
        var gameModeProcPartMock = new Mock<IGameModeProcPart>();
        var renderingServerConnectingProcPartMock = new Mock<IRenderingServerConnectingProcPart>();
        var gameClientWaitConnectingProcPartMock = new Mock<IGameClientWaitConnectionProcPart>();
        ProcPartBinder.Bind(
            gameModeProcPartMock.Object,
            renderingServerConnectingProcPartMock.Object,
            gameClientWaitConnectingProcPartMock.Object);

        // ゲームクライアントモードが選択された
        gameModeProcPartMock.Raise(m => m.OnSelectedGameClientMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        renderingServerConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        gameClientWaitConnectingProcPartMock.VerifyNoOtherCalls();
    }

    [Test]
    public void BindGameClientWaitConnectionActive()
    {
        var gameModeProcPartMock = new Mock<IGameModeProcPart>();
        var renderingServerConnectingProcPartMock = new Mock<IRenderingServerConnectingProcPart>();
        var gameClientWaitConnectingProcPartMock = new Mock<IGameClientWaitConnectionProcPart>();
        ProcPartBinder.Bind(
            gameModeProcPartMock.Object,
            renderingServerConnectingProcPartMock.Object,
            gameClientWaitConnectingProcPartMock.Object);

        // ゲームクライアントモードが選択された
        gameModeProcPartMock.Raise(m => m.OnSelectedRenderingServerMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        gameClientWaitConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        gameClientWaitConnectingProcPartMock.Verify(m => m.StartWaitConnection(), Times.Once);  // 有効になった直後に接続待ち処理を始める
        gameClientWaitConnectingProcPartMock.VerifyNoOtherCalls();
        renderingServerConnectingProcPartMock.VerifyNoOtherCalls();
    }
}
