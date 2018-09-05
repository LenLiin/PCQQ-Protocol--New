    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePackageCommand(QQCommand.Login0x0825)]
    public class LoginPingCommand : ReceiveCommand<Receive_0x0825>
    {
        public LoginPingCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0825(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0825>(client, _packet);
        }

        public override void Receive()
        {
            if (_packet.DataHead == 0xFE)
            {
                _client.OnReceive_0x0825Redirect(_event_args);
            }
            else
            {
                _client.OnReceive_0x0825(_event_args);
            }
        }
    }
}
