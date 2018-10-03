using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Packets.Send.Data;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Data
{
    [ResponsePacketCommand(QQCommand.Data0X0134)]
    public class Data0X0134Command : ResponseCommand<Receive_0X0134>
    {
        public Data0X0134Command(QQEventArgs<Receive_0X0134> args) : base(args)
        {
        }

        public override void Process()
        {
            _service.Send(new Send_0X0195(_user));
            _service.Send(new Send_0X019B(_user));
            _service.Send(new Send_0X01C4(_user));
            _service.Send(new Send_0X01C5(_user));
            _service.Send(new Send_0X00D8(_user));
        }
    }
}