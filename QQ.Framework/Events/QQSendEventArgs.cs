using QQ.Framework.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework
{
    public class QQSendEventArgs: EventArgs
    {
        public QQSendEventArgs(QQClient client, byte[] byteBuffer)
        {
            this.QQClient = client;
            this.byteBuffer = new MemoryStream(byteBuffer);
        }
        /// <summary>
        /// 数据包
        /// </summary>
        public MemoryStream byteBuffer { get; private set; }
        /// <summary>
        /// 客户端实例
        /// </summary>
        public QQClient QQClient { get; private set; }
    }
}
