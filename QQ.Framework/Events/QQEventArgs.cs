using System;
using QQ.Framework.Packets;

namespace QQ.Framework
{
    public class QQEventArgs<R> : EventArgs
        where R : ReceivePacket
    {
        public QQEventArgs(QQClient client, R receivePacket)
        {
            QQClient = client;
            ReceivePacket = receivePacket;
        }

        /// <summary>
        ///     数据包
        /// </summary>
        public R ReceivePacket { get; }

        /// <summary>
        ///     客户端实例
        /// </summary>
        public QQClient QQClient { get; }
    }
}