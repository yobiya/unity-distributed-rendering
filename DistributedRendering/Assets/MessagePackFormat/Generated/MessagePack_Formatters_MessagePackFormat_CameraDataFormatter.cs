// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.MessagePackFormat
{
    using global::System.Buffers;
    using global::MessagePack;

    public sealed class CameraDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePackFormat.CameraData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::MessagePackFormat.CameraData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            formatterResolver.GetFormatterWithVerify<UnityEngine.Vector3>().Serialize(ref writer, value.position, options);
            formatterResolver.GetFormatterWithVerify<UnityEngine.Vector3>().Serialize(ref writer, value.forward, options);
        }

        public global::MessagePackFormat.CameraData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::MessagePackFormat.CameraData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.position = formatterResolver.GetFormatterWithVerify<UnityEngine.Vector3>().Deserialize(ref reader, options);
                        break;
                    case 1:
                        ____result.forward = formatterResolver.GetFormatterWithVerify<UnityEngine.Vector3>().Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name
