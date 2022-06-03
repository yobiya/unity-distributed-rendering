using Moq;
using NUnit.Framework;

namespace RenderingServer
{

public class RenderingServerProcPartTest
{
    private RenderingServerProcPart _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new RenderingServerProcPart();
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null;
    }

    private void VerifyActivate()
    {
    }

    [Test]
    public void Activate()
    {
        _sut.Activate();
    }

    [Test]
    public void Deactivate()
    {
        _sut.Activate();
        _sut.Deactivate();
    }
}

}
