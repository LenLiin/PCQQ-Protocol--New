using System;
using System.IO;

namespace QQ.Framework.Events
{
    public class QQSendEventArgs : EventArgs
    {
        public QQSendEventArgs(QQClient client, byte[] byteBuffer)
        {
            QQClient = client;
            this.ByteBuffer = new MemoryStream(byteBuffer);
        }

        /// <summary>
        ///     数据包
        /// </summary>
        public MemoryStream ByteBuffer { get; }

        /// <summary>
        ///     客户端实例
        /// </summary>
        public QQClient QQClient { get; }
    }
}