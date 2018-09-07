using QQ.Framework.Packets;

namespace QQ.Framework.Domains.Commands
{
    public abstract class ResponseCommand<PacketType> : PacketCommand
        where PacketType : ReceivePacket
    {
        protected readonly QQEventArgs<PacketType> _args;

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