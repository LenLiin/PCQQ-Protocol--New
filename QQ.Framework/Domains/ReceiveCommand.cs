using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QQ.Framework.Packets;

namespace QQ.Framework.Domains
{
    public abstract class ReceiveCommand<PacketType> : PacketCommand
        where PacketType : ReceivePacket
    {
        protected readonly QQClient _client;
        protected PacketType _packet;
        protected QQEventArgs<PacketType> _event_args;

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
