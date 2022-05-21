using Moq;
using NUnit.Framework;

public class GameModeProcPartTest
{
    [Test]
    public void DefaultView()
    {
        var gameModeUIViewControllerMock = new Mock<IGameModeUIViewController>();
        var procPart = new GameModeProcPart(gameModeUIViewControllerMock.Object);

        // 初期状態では、UIは表示されている
        gameModeUIViewControllerMock.VerifySet(m => m.IsActive = true);
    }

    [Test]
    public void SelectGameClientMode()
    {
        var gameModeUIViewControllerMock = new Mock<IGameModeUIViewController>();
        var procPart = new GameModeProcPart(gameModeUIViewControllerMock.Object);

        bool isSelectedGameClientMode = false;
        bool isSelectedRenderingServerMode = false;
        procPart.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;
        procPart.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        // ゲームクライアントモードがUIから選択された
        gameModeUIViewControllerMock.Raise(m => m.OnSelectedGameClientMode += null);

        // モードが選択されたら、UIは非表示になる
        gameModeUIViewControllerMock.VerifySet(m => m.IsActive = false);
        Assert.IsTrue(isSelectedGameClientMode);
        Assert.IsFalse(isSelectedRenderingServerMode);
    }

    [Test]
    public void SelectRenderingServerMode()
    {
        var gameModeUIViewControllerMock = new Mock<IGameModeUIViewController>();
        var procPart = new GameModeProcPart(gameModeUIViewControllerMock.Object);

        bool isSelectedGameClientMode = false;
        bool isSelectedRenderingServerMode = false;
        procPart.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;
        procPart.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        // レンダリングサーバーモードがUIから選択された
        gameModeUIViewControllerMock.Raise(m => m.OnSelectedRenderingServerMode += null);

        // モードが選択されたら、UIは非表示になる
        gameModeUIViewControllerMock.VerifySet(m => m.IsActive = false);
        Assert.IsFalse(isSelectedGameClientMode);
        Assert.IsTrue(isSelectedRenderingServerMode);
    }
}
