using QQ.Framework.Events;
using QQ.Framework.Packets;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains.Commands
{
    public abstract class ReceiveCommand<TPacketType> : IPacketCommand
        where TPacketType : ReceivePacket
    {
        protected readonly ISocketService _service;
        protected readonly IServerMessageSubject _transponder;
        protected readonly QQUser _user;
        protected TPacketType _packet;
        protected QQEventArgs<TPacketType> _eventArgs;

        public ReceiveCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user)
        {
            _service = service;
            _transponder = transponder;
            _user = user;
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
            var responseCommand = ResponsePacketProcessor<TPacketType>.Of(_eventArgs, GetType()).Process();
            responseCommand.Process();
        }
    }
}