using QQ.Framework.Packets;

namespace QQ.Framework.Domains
{
    public abstract class ResponseCommand<PacketType> : PacketCommand
        where PacketType : ReceivePacket
    {
        private readonly QQEventArgs<PacketType> _args;

        public ResponseCommand(QQEventArgs<PacketType> args)
        {
            _args = args;
        }

        /// <summary>
        /// 回复包的处理逻辑
        /// </summary>
        public abstract void Process();
    }
}