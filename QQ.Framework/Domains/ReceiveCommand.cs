using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QQ.Framework.Packets;

namespace QQ.Framework.Domains
{
    public abstract class ReceiveCommand
    {
        protected readonly QQClient _client;

        public ReceiveCommand(byte[] data, QQClient client)
        {
            _client = client;
        }

        /// <summary>
        /// 接收包的处理逻辑在此处完成
        /// </summary>
        public abstract void Receive();
    }
}
