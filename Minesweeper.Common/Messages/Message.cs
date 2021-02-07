using MessagePack;

namespace Minesweeper.Common.Messages
{
    public class Message
    {
        public static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        internal byte[] ToBytes()
        {
            return MessagePackSerializer.Serialize(this, Options);
        }

        internal static Message FromBytes(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<Message>(bytes, Options);
        }
    }
}
