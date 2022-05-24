using Moq;
using NUnit.Framework;

namespace Common
{

public class ProcPartBinderTest
{
    private struct MockCollection
    {
        public Mock<IGameModeProcPart> gameModeProcPartMock;
        public Mock<IRenderingServerConnectingProcPart> renderingServerConnectingProcPartMock;
        public Mock<IGameClientWaitConnectionProcPart> gameClientWaitConnectingProcPartMock;
    }

    private MockCollection CreateMockCollectionAndBind()
    {
        var collection = new MockCollection();

        var gameModeProcPartMock = new Mock<IGameModeProcPart>();
        var renderingServerConnectingProcPartMock = new Mock<IRenderingServerConnectingProcPart>();
        var gameClientWaitConnectingProcPartMock = new Mock<IGameClientWaitConnectionProcPart>();

        ProcPartBinder.Bind(
            gameModeProcPartMock.Object,
            renderingServerConnectingProcPartMock.Object,
            gameClientWaitConnectingProcPartMock.Object);

        collection.gameModeProcPartMock = gameModeProcPartMock;
        collection.renderingServerConnectingProcPartMock = renderingServerConnectingProcPartMock;
        collection.gameClientWaitConnectingProcPartMock = gameClientWaitConnectingProcPartMock;

        return collection;
    }

    [Test]
    public void BindRenderingServerConnectingActive()
    {
        var collection = CreateMockCollectionAndBind();

        // ゲームクライアントモードが選択された
        collection.gameModeProcPartMock.Raise(m => m.OnSelectedGameClientMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        collection.renderingServerConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        collection.gameClientWaitConnectingProcPartMock.VerifyNoOtherCalls();
    }

    [Test]
    public void BindGameClientWaitConnectionActive()
    {
        var collection = CreateMockCollectionAndBind();

        // ゲームクライアントモードが選択された
        collection.gameModeProcPartMock.Raise(m => m.OnSelectedRenderingServerMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        collection.gameClientWaitConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        collection.gameClientWaitConnectingProcPartMock.Verify(m => m.StartWaitConnection(), Times.Once);   // 有効になった直後に接続待ち処理を始める
        collection.gameClientWaitConnectingProcPartMock.VerifyNoOtherCalls();
        collection.renderingServerConnectingProcPartMock.VerifyNoOtherCalls();
    }
}

}
