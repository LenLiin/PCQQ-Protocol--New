using QQ.Framework.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework
{
    public class QQEventArgs<R> : EventArgs
        where R : ReceivePacket
    {
        public QQEventArgs(QQClient client,R receivePacket)
        {
            this.QQClient = client;
            this.ReceivePacket = receivePacket;
        }
        /// <summary>
        /// 数据包
        /// </summary>
        public R ReceivePacket { get; private set; }
        /// <summary>
        /// 客户端实例
        /// </summary>
        public QQClient QQClient { get; private set; }
    }
}
