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
    /// 获取QQ等级
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0x005C)]
    public class GetQQLevelCommand : ReceiveCommand<Receive_0x005C>
    {
        /// <summary>
        /// 获取QQ等级
        /// </summary>
        public GetQQLevelCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x005C(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x005C>(client, _packet);
        }

        public override void Process()
        {
            _client.OnReceive_0x005C(_event_args);
            
        }
    }
}
