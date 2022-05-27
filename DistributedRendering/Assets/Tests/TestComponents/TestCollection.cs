using Common;

public class TestCollection<T>
{
    public T sut;
    public MockServiceLocator serviceLocator;

    public TestCollection(T sut, MockServiceLocator serviceLocator)
    {
        this.sut = sut;
        this.serviceLocator = serviceLocator;
    }
}
