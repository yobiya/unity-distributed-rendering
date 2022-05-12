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

        public event Action OnRequestConnecting;

        public void ShowConnecting() => isCalledShowConnecting = true;
        public void ShowConnected() => isCalledShowConnected = true;

        // テスト用メソッド
        public void RequestConnect() => OnRequestConnecting?.Invoke();
    }

    class TestNamedPipeClient : INamedPipeClient
    {
        public bool isCalledConnect = false;

        public event Action OnConnected;

        public void Connect(int timeOutTime) => isCalledConnect = true;
        public void Write(string text) {}

        // テスト用メソッド
        public void SuccessConnect() => OnConnected?.Invoke();
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
        var namedPipeClient = new TestNamedPipeClient();
        namedPipeClient.OnConnected += () => isConnected = true;

        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        Assert.IsFalse(isConnected);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);

        renderingServerConnectingUIViewController.RequestConnect();

        // 接続中
        Assert.IsFalse(isConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);

        namedPipeClient.SuccessConnect();

        // 接続成功
        Assert.IsTrue(isConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnected);
    }
}
