using System;
using Moq;
using NUnit.Framework;

namespace Common
{

public class GameModeUIControllerTest
{
    private (GameModeUIController, MockServiceLocator) CreateSUT()
    {
        var mockLocator = new MockServiceLocator();
        mockLocator.RegisterMock<IGameModeUI>();

        var gameModeUIMock = mockLocator.GetMock<IGameModeUI>();
        gameModeUIMock.SetupAdd(m => m.OnSelectedGameClient += It.IsAny<Action>());
        gameModeUIMock.SetupAdd(m => m.OnSelectedRenderingServer += It.IsAny<Action>());

        var sut = new GameModeUIController(gameModeUIMock.Object);
        sut.Activate();

        gameModeUIMock.Verify(m => m.Activate(), Times.Once);
        gameModeUIMock.VerifyAdd(m => m.OnSelectedGameClient += It.IsAny<Action>(), Times.Once);
        gameModeUIMock.VerifyAdd(m => m.OnSelectedRenderingServer += It.IsAny<Action>(), Times.Once);

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
        sut.OnSelectedGameClientMode += () => isSelectedGameClientMode = true;

        // ボタンが押されたら、OnSelectedGameClientイベントが呼ばれる
        sl.GetMock<IGameModeUI>().Raise(m => m.OnSelectedGameClient += null);

        Assert.IsTrue(isSelectedGameClientMode);
        sl.VerifyNoOtherCallsAll();
    }

    [Test]
    public void SelectRenderingServerMode()
    {
        var (sut, sl) = CreateSUT();

        bool isSelectedRenderingServerMode = false;
        sut.OnSelectedRenderingServerMode += () => isSelectedRenderingServerMode = true;

        // ボタンが押されたら、OnSelectedRenderingServerModeイベントが呼ばれる
        sl.GetMock<IGameModeUI>().Raise(m => m.OnSelectedRenderingServer += null);

        Assert.IsTrue(isSelectedRenderingServerMode);
        sl.VerifyNoOtherCallsAll();
    }
}

}
