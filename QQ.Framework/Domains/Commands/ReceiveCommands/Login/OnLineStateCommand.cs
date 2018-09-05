    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePackageCommand(QQCommand.Login0x00EC)]
    public class OnLineStateCommand : ReceiveCommand<Receive_0x00EC>
    {
        public OnLineStateCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x00EC(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x00EC>(client, _packet);
        }

        public override void Receive()
        {
            _client.OnReceive_0x00EC(_event_args);
        }
    }
}
