using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConnectUIViewControllerTest
{
    [Test]
    public void ConnectUIViewController()
    {
        var controller = new RenderingServerConnectingUIViewController();

        bool isRequestConnecting = false;
        controller.OnRequestConnecting += () => isRequestConnecting = true;

        Assert.IsTrue(isRequestConnecting);
    }
}
