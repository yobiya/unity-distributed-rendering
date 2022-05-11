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

        public bool IsConnected { get; private set; } = false;

        public void Connect() => isCalledConnect = true;
        public void Write(string text) {}
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
        var namedPipeClient = new TestNamedPipeClient();
        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        renderingServerConnectingUIViewController.RequestConnect();

        Assert.IsFalse(namedPipeClient.IsConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
    }

    [Test]
    public void ConnectSuccessTest()
    {
        var renderingServerConnectingUIViewController = new TestRenderingServerConnectingUIViewController();
        var namedPipeClient = new TestNamedPipeClient();
        var procPart = new RenderingServerConnectingProcPart(renderingServerConnectingUIViewController, namedPipeClient);

        renderingServerConnectingUIViewController.RequestConnect();

        Assert.IsFalse(namedPipeClient.IsConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);

        procPart.Update(1.0f);

        // 接続中
        Assert.IsFalse(namedPipeClient.IsConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnecting);
        Assert.IsFalse(renderingServerConnectingUIViewController.isCalledShowConnected);

        procPart.Update(1.0f);

        // 接続成功
        Assert.IsTrue(namedPipeClient.IsConnected);
        Assert.IsTrue(renderingServerConnectingUIViewController.isCalledShowConnected);
    }
}
