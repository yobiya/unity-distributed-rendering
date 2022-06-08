using System;
using MessagePack;

namespace Common
{

public interface ISerializer
{
    byte[] Serialize<T>(T source);
    T Deserialize<T>(ReadOnlyMemory<byte> data);
}

public class MessagePackSerializerWrapper : ISerializer
{
    public byte[] Serialize<T>(T source)
    {
        return MessagePackSerializer.Serialize(source);
    }

    public T Deserialize<T>(ReadOnlyMemory<byte> data)
    {
        return MessagePackSerializer.Deserialize<T>(data);
    }
}

}
