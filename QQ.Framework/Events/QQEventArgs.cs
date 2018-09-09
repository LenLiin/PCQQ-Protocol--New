using System;
using QQ.Framework.Domains;
using QQ.Framework.Packets;

namespace QQ.Framework
{
    public class QQEventArgs<R> : EventArgs
        where R : ReceivePacket
    {
        private readonly SocketService _service;
        private readonly QQUser _user;

        public QQEventArgs(SocketService service, QQUser user, R receivePacket)
        {
            _service = service;
            _user = user;
            ReceivePacket = receivePacket;
        }

        /// <summary>
        ///     数据包
        /// </summary>
        public R ReceivePacket { get; }

        /// <summary>
        ///     Socket服务
        /// </summary>
        public SocketService Service { get { return _service; } }

        /// <summary>
        ///     账号信息
        /// </summary>
        public QQUser User { get { return _user; } }
    }
}