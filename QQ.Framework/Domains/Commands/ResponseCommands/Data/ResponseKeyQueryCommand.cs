using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Packets.Send.Data;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Data
{
    [ResponsePacketCommand(QQCommand.Data0x001D)]
    public class ResponseKeyQueryCommand : ResponseCommand<Receive_0x001D>
    {
        public ResponseKeyQueryCommand(QQEventArgs<Receive_0x001D> args) : base(args)
        {
        }

        public override void Process()
        {
            _service.Send(new Send_0x005C(_user));
        }
    }
}