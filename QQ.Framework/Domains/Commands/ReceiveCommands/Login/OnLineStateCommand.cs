using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x00EC)]
    public class OnLineStateCommand : ReceiveCommand<Receive_0x00EC>
    {
        public OnLineStateCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0x00EC(data, _user);
            _event_args = new QQEventArgs<Receive_0x00EC>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}