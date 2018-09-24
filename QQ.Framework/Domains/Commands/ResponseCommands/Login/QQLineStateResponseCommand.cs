using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Data;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0X00Ec)]
    public class QQLineStateResponseCommand : ResponseCommand<Receive_0X00Ec>
    {
        public QQLineStateResponseCommand(QQEventArgs<Receive_0X00Ec> args) : base(args)
        {
        }

        public override void Process()
        {
            _service.Send(new Send_0X001D(_user));
            _service.Send(new Send_0X0195(_user));
            _service.Send(new Send_0X019B(_user));
        }
    }
}