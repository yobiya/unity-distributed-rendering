using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private Dictionary<Type, object> _instanceContainer = new Dictionary<Type, object>();

    public void Set<T>(object instance)
    {
        _instanceContainer.Add(typeof(T), instance);
    }

    public T Get<T>()
    {
        return (T)_instanceContainer[typeof(T)];
    }
}
