    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePackageCommand(QQCommand.Login0x00BA)]
    public class VerifyCodeCommand : ReceiveCommand<Receive_0x00BA>
    {
        public VerifyCodeCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x00BA(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x00BA>(client, _packet);
        }

        public override void Receive()
        {
            _client.OnReceive_0x00BA(_event_args);
            
        }
    }
}
