using System;
using QQ.Framework.Domains;
using QQ.Framework.Packets;

namespace QQ.Framework.Events
{
    public class QQEventArgs<TR> : EventArgs
        where TR : ReceivePacket
    {
        public QQEventArgs(ISocketService service, QQUser user, TR receivePacket)
        {
            Service = service;
            User = user;
            ReceivePacket = receivePacket;
        }

        /// <summary>
        ///     数据包
        /// </summary>
        public TR ReceivePacket { get; }

        /// <summary>
        ///     Socket服务
        /// </summary>
        public ISocketService Service { get; }

        /// <summary>
        ///     账号信息
        /// </summary>
        public QQUser User { get; }
    }
}