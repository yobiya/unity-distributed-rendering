using System;
using MessagePack;

namespace Common
{

public interface ISerializer
{
    T Deserialize<T>(ReadOnlyMemory<byte> data);
}

public class MessagePackSerializerWrapper : ISerializer
{
    public T Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        return MessagePackSerializer.Deserialize<T>(data);
    }
}

}
