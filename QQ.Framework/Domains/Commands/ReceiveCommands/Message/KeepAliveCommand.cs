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
    /// KeepAlive（心跳）
    /// </summary>
    [ReceivePackageCommand(QQCommand.Message0x0058)]
    public class KeepAliveCommand : ReceiveCommand<Receive_0x0058>
    {
        /// <summary>
        /// KeepAlive（心跳）
        /// </summary>
        public KeepAliveCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0058(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0058>(client, _packet);
        }

        public override void Receive()
        {
            _client.OnReceive_0x0058(_event_args);
            
        }
    }
}
