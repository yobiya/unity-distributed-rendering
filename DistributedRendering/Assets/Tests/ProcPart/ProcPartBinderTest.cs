using Moq;
using NUnit.Framework;

public class ProcPartBinderTest
{
    [Test]
    public void BindRenderingServerConnectingActive()
    {
        var gameModeProcPartMock = new Mock<IGameModeProcPart>();
        var renderingServerConnectingProcPartMock = new Mock<IRenderingServerConnectingProcPart>();
        ProcPartBinder.Bind(gameModeProcPartMock.Object, renderingServerConnectingProcPartMock.Object);

        // ゲームクライアントモードが選択された
        gameModeProcPartMock.Raise(m => m.OnSelectedGameClientMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        renderingServerConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
    }
}
