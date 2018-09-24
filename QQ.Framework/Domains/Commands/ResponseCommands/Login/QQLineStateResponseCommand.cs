using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Data;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0x00EC)]
    public class QQLineStateResponseCommand : ResponseCommand<Receive_0x00EC>
    {
        public QQLineStateResponseCommand(QQEventArgs<Receive_0x00EC> args) : base(args)
        {
        }

        public override void Process()
        {
            _service.Send(new Send_0x001D(_user));
        }
    }
}