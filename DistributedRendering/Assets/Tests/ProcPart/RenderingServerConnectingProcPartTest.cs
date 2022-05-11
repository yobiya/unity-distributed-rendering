using System;
using NUnit.Framework;
using UnityEngine.TestTools;

public class RenderingServerConnectingProcPartTest
{
    class TestRenderingServerConnectingUIViewController : IRenderingServerConnectingUIViewController
    {
        public event Action OnRequestConnecting;

        public void RequestConnect() => OnRequestConnecting?.Invoke();
    }

    class TestNamedPipeClient : INamedPipeClient
    {
        public bool isCalledConnect = false;

        public bool IsConnecting { get; }

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
}
