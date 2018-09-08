using QQ.Framework.Packets;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains.Commands
{
    public abstract class ReceiveCommand<PacketType> : PacketCommand
        where PacketType : ReceivePacket
    {
        protected readonly QQClient _client;
        protected PacketType _packet;
        protected QQEventArgs<PacketType> _event_args;

        public ReceiveCommand(byte[] data, QQClient client)
        {
            _client = client;
        }

        /// <summary>
        ///     接收包的处理逻辑在此处完成
        /// </summary>
        public abstract void Process();

        /// <summary>
        ///     发送响应包
        /// </summary>
        protected void Response()
        {
            var response_command = ResponsePacketProcessor<PacketType>.of(_event_args, GetType()).Process();
            response_command.Process();
        }
    }
}