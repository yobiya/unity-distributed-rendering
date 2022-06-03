using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace RenderingServer
{

public class RenderingServerProcPartTest
{
    private RenderingServerProcPart _sut;
    private Mock<INamedPipeServer> _namedPipeServerMock;
    private Mock<ISyncCameraViewController> _syncCameraViewControllerMock;

    [SetUp]
    public void SetUp()
    {
        _namedPipeServerMock = new Mock<INamedPipeServer>();
        _syncCameraViewControllerMock = new Mock<ISyncCameraViewController>();
        _sut = new RenderingServerProcPart(_namedPipeServerMock.Object, _syncCameraViewControllerMock.Object);

        _namedPipeServerMock.SetupAdd(m => m.OnRecieved += It.IsAny<Action<string>>());
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null;
        _namedPipeServerMock = null;
        _syncCameraViewControllerMock = null;
    }

    private void VerifyActivate()
    {
        _namedPipeServerMock.VerifyAdd(m => m.OnRecieved += It.IsAny<Action<string>>(), Times.Once);
        _syncCameraViewControllerMock.Verify(m => m.Activate(), Times.Once);
        _namedPipeServerMock.Verify(m => m.ReadCommandAsync(), Times.Once);
    }

    [UnityTest]
    public IEnumerator Activate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.Activate();

        VerifyActivate();
    });

    [UnityTest]
    public IEnumerator Deactivate() => UniTask.ToCoroutine(async () =>
    {
        await _sut.Activate();
        _sut.Deactivate();

        VerifyActivate();
    });
}

}
