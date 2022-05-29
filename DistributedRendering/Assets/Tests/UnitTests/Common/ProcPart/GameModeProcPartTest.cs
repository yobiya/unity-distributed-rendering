using System;
using Moq;
using NUnit.Framework;

namespace Common
{

public class GameModeProcPartTest
{
    (GameModeProcPart, MockServiceLocator) CreateSUT()
    {
        var mockLocator = new MockServiceLocator();
        mockLocator.RegisterMock<IGameModeUIController>();

        var uiViewControllerMock = mockLocator.GetMock<IGameModeUIController>();
        uiViewControllerMock.SetupAdd(m => m.OnSelectedGameClientMode += It.IsAny<Action>());
        uiViewControllerMock.SetupAdd(m => m.OnSelectedRenderingServerMode+= It.IsAny<Action>());

        var sut = new GameModeProcPart(mockLocator);
        sut.Activate();

        uiViewControllerMock.Verify(m => m.Activate());
        uiViewControllerMock.VerifyAdd(m => m.OnSelectedGameClientMode += It.IsAny<Action>(), Times.Once);
        uiViewControllerMock.VerifyAdd(m => m.OnSelectedRenderingServerMode += It.IsAny<Action>(), Times.Once);

        return (sut, mockLocator);
    }

    [Test]
    public void Activate()
    {
        var (sut, sl) = CreateSUT();

        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void SelectGameClientMode()
    {
        var (sut, sl) = CreateSUT();

        bool isSelectedGameClientMode = false;
        bool isSelectedRenderingServerMode = false;
        sut.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;
        sut.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        // ゲームクライアントモードがUIから選択された
        sl.GetMock<IGameModeUIController>().Raise(m => m.OnSelectedGameClientMode += null);

        // モードが選択されたら、UIは非表示になる
        sl.GetMock<IGameModeUIController>().Verify(m => m.Deactivate());
        Assert.IsTrue(isSelectedGameClientMode);
        Assert.IsFalse(isSelectedRenderingServerMode);

        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void SelectRenderingServerMode()
    {
        var (sut, sl) = CreateSUT();

        bool isSelectedGameClientMode = false;
        bool isSelectedRenderingServerMode = false;
        sut.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;
        sut.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        // レンダリングサーバーモードがUIから選択された
        sl.GetMock<IGameModeUIController>().Raise(m => m.OnSelectedRenderingServerMode += null);

        // モードが選択されたら、UIは非表示になる
        sl.GetMock<IGameModeUIController>().Verify(m => m.Deactivate());
        Assert.IsFalse(isSelectedGameClientMode);
        Assert.IsTrue(isSelectedRenderingServerMode);
    }
}

}