using System;
using NUnit.Framework;

public class GameModeProcPartTest
{
    private class TestGameModeUIViewController : IGameModeUIViewController
    {
        public bool IsActive { get; set; }

        public event Action OnSelectedGameClientMode;
        public event Action OnSelectedRenderingServerMode;

        // テスト用メソッド
        public void SelectGameClientMode() => OnSelectedGameClientMode?.Invoke();
        public void SelectRenderingServerMode() => OnSelectedRenderingServerMode?.Invoke();
    }

    public class TestRenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
    {
        public bool IsActive { get; set; } = false;

        public event Action OnRequestConnecting;

        public void Reset() {}
        public void ShowConnected() {}
        public void ShowConnecting() {}
        public void ShowFailed() {}
    }

    public class TestGameClientConnectingWaitUIViewController : IGameClientConnectingWaitUIViewController
    {
        public bool IsActive { get; set; } = false;
    }

    [Test]
    public void StartView()
    {
        var gameModeUIViewController = new TestGameModeUIViewController();
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();
        var gameClientConnectingWaitUIViewController = new TestGameClientConnectingWaitUIViewController();
        var procPart = new GameModeProcPart(gameModeUIViewController, renderingServerConnectingUIViewController, gameClientConnectingWaitUIViewController);

        Assert.IsTrue(gameModeUIViewController.IsActive);
        Assert.IsFalse(renderingServerConnectingUIViewController.IsActive);
        Assert.IsFalse(gameClientConnectingWaitUIViewController.IsActive);
    }

    [Test]
    public void StartGameClientMode()
    {
        var gameModeUIViewController = new TestGameModeUIViewController();
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();
        var gameClientConnectingWaitUIViewController = new TestGameClientConnectingWaitUIViewController();
        var procPart = new GameModeProcPart(gameModeUIViewController, renderingServerConnectingUIViewController, gameClientConnectingWaitUIViewController);

        Assert.IsTrue(gameModeUIViewController.IsActive);
        Assert.IsFalse(renderingServerConnectingUIViewController.IsActive);
        Assert.IsFalse(gameClientConnectingWaitUIViewController.IsActive);

        gameModeUIViewController.SelectGameClientMode();

        Assert.IsFalse(gameModeUIViewController.IsActive);
        Assert.IsTrue(renderingServerConnectingUIViewController.IsActive);
        Assert.IsFalse(gameClientConnectingWaitUIViewController.IsActive);
    }

    [Test]
    public void StartRenderingServerMode()
    {
        var gameModeUIViewController = new TestGameModeUIViewController();
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();
        var gameClientConnectingWaitUIViewController = new TestGameClientConnectingWaitUIViewController();
        var procPart = new GameModeProcPart(gameModeUIViewController, renderingServerConnectingUIViewController, gameClientConnectingWaitUIViewController);

        Assert.IsFalse(renderingServerConnectingUIViewController.IsActive);
        Assert.IsFalse(gameClientConnectingWaitUIViewController.IsActive);

        gameModeUIViewController.SelectRenderingServerMode();

        Assert.IsFalse(gameModeUIViewController.IsActive);
        Assert.IsFalse(renderingServerConnectingUIViewController.IsActive);
        Assert.IsTrue(gameClientConnectingWaitUIViewController.IsActive);
    }
}
