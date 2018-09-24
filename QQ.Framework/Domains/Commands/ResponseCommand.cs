using QQ.Framework.Events;
using QQ.Framework.Packets;

namespace QQ.Framework.Domains.Commands
{
    public abstract class ResponseCommand<TPacketType> : IPacketCommand
        where TPacketType : ReceivePacket
    {
        protected readonly TPacketType _packet;
        protected readonly ISocketService _service;
        protected QQUser _user;

        public ResponseCommand(QQEventArgs<TPacketType> args)
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