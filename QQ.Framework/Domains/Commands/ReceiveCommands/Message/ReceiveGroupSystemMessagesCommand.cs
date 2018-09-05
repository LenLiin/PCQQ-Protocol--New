    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    /// <summary>
    /// 收到群/系统消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x0017)]
    public class ReceiveGroupSystemMessagesCommand : ReceiveCommand<Receive_0x0017>
    {
        /// <summary>
        /// 收到群/系统消息
        /// </summary>
        public ReceiveGroupSystemMessagesCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0017(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0017>(client, _packet);
        }

        public override void Process()
        {
            _client.OnReceive_0x0017(_event_args);
        }
    }
}
