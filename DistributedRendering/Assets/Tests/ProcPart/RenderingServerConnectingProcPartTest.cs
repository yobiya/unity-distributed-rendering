using System;
using NUnit.Framework;
using UnityEngine.TestTools;

public class RenderingServerConnectingProcPartTest
{
    class TestRenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
    {
        // テスト用変数
        public bool isCalledShowConnecting = false;
        public bool isCalledShowConnected = false;
        public bool isCalledShowFailed = false;

        public event Action OnRequestConnecting;

        public void ShowConnecting() => isCalledShowConnecting = true;
        public void ShowConnected() => isCalledShowConnected = true;
        public void ShowFailed() => isCalledShowFailed = true;

        // テスト用メソッド
        public void RequestConnect() => OnRequestConnecting?.Invoke();
    }

    class TestNamedPipeClient : INamedPipeClient
    {
        public bool isCalledConnect = false;

        public event Action OnConnected;
        public event Action OnFailed;

        public void Connect(int timeOutTime) => isCalledConnect = true;
        public void Write(string text) {}

        // テスト用メソッド
        public void SuccessConnect() => OnConnected?.Invoke();
        public void FailConnect() => OnFailed?.Invoke();
    }

    [Test]
    public void StartConnectTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();
        var namedPipeClient = new TestNamedPipeClient();
        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        renderingServerConnectingUIViewController.RequestConnect();

        Assert.IsTrue(namedPipeClient.isCalledConnect);
    }

    [Test]
    public void ConnectingTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        renderingServerConnectingUIViewController.RequestConnect();

        Assert.IsFalse(isConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);

        renderingServerConnectingUIViewController.RequestConnect();

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);

        namedPipeClient.SuccessConnect();

        // 接続成功
        Assert.IsTrue(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);
    }

    [Test]
    public void ConnectTimeOutTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();

        bool isConnected = false;
        bool isFailed = false;
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;
        namedPipeClient.OnFailed += () => isFailed = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);

        renderingServerConnectingUIViewController.RequestConnect();

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsFalse(isFailed);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowFailed);

        namedPipeClient.FailConnect();

        // 接続失敗
        Assert.IsFalse(isConnected);
        Assert.IsTrue(isFailed);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowFailed);
    }
}
