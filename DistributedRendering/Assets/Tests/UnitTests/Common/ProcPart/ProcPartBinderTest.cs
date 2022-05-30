using System;
using Moq;
using NUnit.Framework;
using RenderingServer;
using GameClient;

namespace Common
{

public class ProcPartBinderTest
{
    private struct MockCollection
    {
        public Mock<IGameModeProcPart> gameModeProcPartMock;
        public Mock<IRenderingServerConnectingProcPart> renderingServerConnectingProcPartMock;
        public Mock<IGameClientWaitConnectionProcPart> gameClientWaitConnectingProcPartMock;

        public void VerifyNoOtherCallsAllMocks()
        {
            gameModeProcPartMock.VerifyNoOtherCalls();
            renderingServerConnectingProcPartMock.VerifyNoOtherCalls();
            gameClientWaitConnectingProcPartMock.VerifyNoOtherCalls();
        }
    }

    private (MockCollection, MockServiceLocator) CreateMockCollectionAndBind()
    {
        var mockLocator = new MockServiceLocator();
        mockLocator.RegisterMock<IOffscreenRenderingProcPart>();
        mockLocator.RegisterMock<IDebugRenderingProcPart>();
        mockLocator.RegisterMock<IResponseRenderingProcPart>();

        var collection = new MockCollection();

        var gameModeProcPartMock = new Mock<IGameModeProcPart>();
        var renderingServerConnectingProcPartMock = new Mock<IRenderingServerConnectingProcPart>();
        var gameClientWaitConnectingProcPartMock = new Mock<IGameClientWaitConnectionProcPart>();

        {
            gameModeProcPartMock.SetupAdd(m => m.OnSelectedGameClientMode += It.IsAny<Action>());
            gameModeProcPartMock.SetupAdd(m => m.OnSelectedRenderingServerMode += It.IsAny<Action>());
            gameClientWaitConnectingProcPartMock.SetupAdd(m => m.OnConnected += It.IsAny<Action>());
            mockLocator.GetMock<IOffscreenRenderingProcPart>().SetupAdd(m => m.OnActivated += It.IsAny<Action<IRenderTextureView>>());
        }

        ProcPartBinder.Bind(
            mockLocator,
            gameModeProcPartMock.Object,
            renderingServerConnectingProcPartMock.Object,
            gameClientWaitConnectingProcPartMock.Object);

        {
            gameModeProcPartMock.VerifyAdd(m => m.OnSelectedGameClientMode += It.IsAny<Action>(), Times.Once);
            gameModeProcPartMock.VerifyAdd(m => m.OnSelectedRenderingServerMode += It.IsAny<Action>(), Times.Once);
            gameClientWaitConnectingProcPartMock.VerifyAdd(m => m.OnConnected += It.IsAny<Action>(), Times.Once);
            mockLocator.GetMock<IOffscreenRenderingProcPart>().VerifyAdd(m => m.OnActivated += It.IsAny<Action<IRenderTextureView>>(), Times.Once);
        }

        collection.gameModeProcPartMock = gameModeProcPartMock;
        collection.renderingServerConnectingProcPartMock = renderingServerConnectingProcPartMock;
        collection.gameClientWaitConnectingProcPartMock = gameClientWaitConnectingProcPartMock;

        return (collection, mockLocator);
    }

    [Test]
    public void BindRenderingServerConnectingActive()
    {
        var (collection, sl) = CreateMockCollectionAndBind();

        // ゲームクライアントモードが選択された
        collection.gameModeProcPartMock.Raise(m => m.OnSelectedGameClientMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        collection.gameModeProcPartMock.Verify(m => m.Deactivate(), Times.Once);
        collection.renderingServerConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        collection.VerifyNoOtherCallsAllMocks();
        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void BindGameClientWaitConnectionActive()
    {
        var (collection, sl) = CreateMockCollectionAndBind();

        // ゲームクライアントモードが選択された
        collection.gameModeProcPartMock.Raise(m => m.OnSelectedRenderingServerMode += null);

        // レンダリングサーバーへの接続機能が有効になる
        collection.gameModeProcPartMock.Verify(m => m.Deactivate(), Times.Once);
        collection.gameClientWaitConnectingProcPartMock.Verify(m => m.Activate(), Times.Once);
        collection.gameClientWaitConnectingProcPartMock.Verify(m => m.StartWaitConnection(), Times.Once);   // 有効になった直後に接続待ち処理を始める
        collection.VerifyNoOtherCallsAllMocks();
        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void BindRenderingServerOffscreenRenderingActive()
    {
        var (collection, sl) = CreateMockCollectionAndBind();

        // クライアントがレンダリングサーバーに接続すると
        // オフスクリーンレンダリングが有効になる
        collection.gameClientWaitConnectingProcPartMock.Raise(m => m.OnConnected += null);
        sl.GetMock<IOffscreenRenderingProcPart>().Verify(m => m.Activate(), Times.Once);
        sl.GetMock<IResponseRenderingProcPart>().Verify(m => m.Activate(), Times.Once);
        collection.VerifyNoOtherCallsAllMocks();
        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void BindRenderingServerDebugRenderingActive()
    {
        var (collection, sl) = CreateMockCollectionAndBind();

        // オフスクリーンレンダリングが有効になったら、デバッグレンダリングが有効になる
        sl.GetMock<IOffscreenRenderingProcPart>().Raise(m => m.OnActivated += null, new object[] { null });
        sl.GetMock<IDebugRenderingProcPart>().Verify(m => m.Activate(null), Times.Once);
        collection.VerifyNoOtherCallsAllMocks();
        sl.VerifyNoOtherCallsAll();
    }
}

}
