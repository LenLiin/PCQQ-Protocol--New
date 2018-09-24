using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Packets.Send.Data;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Data
{
    [ResponsePacketCommand(QQCommand.Data0X001D)]
    public class ResponseKeyQueryCommand : ResponseCommand<Receive_0X001D>
    {
        public ResponseKeyQueryCommand(QQEventArgs<Receive_0X001D> args) : base(args)
        {
        }

        public override void Process()
        {
            _service.Send(new Send_0X005C(_user));
        }
    }
}