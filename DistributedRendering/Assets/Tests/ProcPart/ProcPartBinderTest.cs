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
        gameClientWaitConnectingProcPartMock.Verify(m => m.Activate(), Times.Never);
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
        renderingServerConnectingProcPartMock.Verify(m => m.Activate(), Times.Never);
        gameClientWaitConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
    }
}
