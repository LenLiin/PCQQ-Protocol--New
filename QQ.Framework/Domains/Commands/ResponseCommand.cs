using QQ.Framework.Packets;

namespace QQ.Framework.Domains.Commands
{
    public abstract class ResponseCommand<PacketType> : PacketCommand
        where PacketType : ReceivePacket
    {
        protected readonly PacketType _packet;
        protected readonly SocketService _service;
        protected QQUser _user;

        public ResponseCommand(QQEventArgs<PacketType> args)
        {
            _packet = args.ReceivePacket;
            _service = args.Service;
            _user = args.User;
        }

        /// <summary>
        ///     回复包的处理逻辑
        /// </summary>
        public abstract void Process();
    }
}