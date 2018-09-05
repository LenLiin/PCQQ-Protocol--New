    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0828)]
    public class PreLoginCommand : ReceiveCommand<Receive_0x0828>
    {
        public PreLoginCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0828(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0828>(client, _packet);
        }

        public override void Process()
        {
            _client.OnReceive_0x0828(_event_args);

            //定时发送心跳包
            var timersInvoke = new TimersInvoke(_client);
            timersInvoke.StartTimer();
            
        }
    }
}
