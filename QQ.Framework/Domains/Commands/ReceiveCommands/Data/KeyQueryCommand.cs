    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    /// <summary>
    /// Key查询
    /// </summary>
    [ReceivePackageCommand(QQCommand.Data0x001D)]
    public class KeyQueryCommand : ReceiveCommand<Receive_0x001D>
    {
        /// <summary>
        /// Key查询
        /// </summary>
        public KeyQueryCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x001D(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x001D>(client, _packet);
        }

        public override void Receive()
        {
            _client.OnReceive_0x001D(_event_args);
            
        }
    }
}
