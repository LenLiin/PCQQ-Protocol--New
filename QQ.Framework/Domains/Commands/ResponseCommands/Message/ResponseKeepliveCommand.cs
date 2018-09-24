using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    [ResponsePacketCommand(QQCommand.Message0X0058)]
    public class ResponseKeepliveCommand : ResponseCommand<Receive_0X0058>
    {
        public ResponseKeepliveCommand(QQEventArgs<Receive_0X0058> args) : base(args)
        {
        }

        public override void Process()
        {
        }
    }
}