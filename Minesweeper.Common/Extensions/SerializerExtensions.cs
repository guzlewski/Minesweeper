using MessagePack;

namespace Minesweeper.Common.Extensions
{
    public static class SerializerExtensions
    {
        public static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static byte[] Serialize<T>(this T t)
        {
            return MessagePackSerializer.Serialize(t, Options);
        }

        public static T Deserialize<T>(this byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes, Options);
        }
    }
}
