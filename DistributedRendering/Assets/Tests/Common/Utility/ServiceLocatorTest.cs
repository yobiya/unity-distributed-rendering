using NUnit.Framework;

namespace Common
{

interface DummyInterface
{
}

class DummyClass : DummyInterface
{
}

public class ServiceLocatorTest
{
    [Test]
    public void SetGet()
    {
        var sut = new ServiceLocator();

        var clazz = new DummyClass();
        sut.Set<DummyInterface>(clazz);
        DummyInterface target = sut.Get<DummyInterface>();

        Assert.AreEqual(clazz, target);
    }
}

}
