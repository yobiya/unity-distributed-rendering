using System;
using System.Collections.Generic;
using Moq;

namespace Common
{

public class MockServiceLocator : ServiceLocator
{
    private Dictionary<Type, object> _mockContainer = new Dictionary<Type, object>();

    public void RegisterMock<T>() where T : class
    {
        var mock = new Mock<T>();
        _mockContainer.Add(typeof(T), mock);
        Set<T>(mock.Object);
    }

    public Mock<T> GetMock<T>() where T : class
    {
        return (Mock<T>)_mockContainer[typeof(T)];
    }

    public void VerifyNoOtherCallsAll()
    {
        foreach (var mock in _mockContainer.Values)
        {
            var type = mock.GetType();
            var method = type.GetMethod("VerifyNoOtherCalls");
            method.Invoke(mock, new object[0]);
        }
    }
}

}
