using System;
using QQ.Framework.Domains;
using QQ.Framework.Packets;

namespace QQ.Framework.Events
{
    public class QQEventArgs<R> : EventArgs
        where R : ReceivePacket
    {
        public QQEventArgs(SocketService service, QQUser user, R receivePacket)
        {
            Service = service;
            User = user;
            ReceivePacket = receivePacket;
        }

        /// <summary>
        ///     数据包
        /// </summary>
        public R ReceivePacket { get; }

        /// <summary>
        ///     Socket服务
        /// </summary>
        public SocketService Service { get; }

        /// <summary>
        ///     账号信息
        /// </summary>
        public QQUser User { get; }
    }
}