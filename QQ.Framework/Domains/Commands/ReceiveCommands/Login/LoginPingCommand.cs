using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0825)]
    public class LoginPingCommand : ReceiveCommand<Receive_0x0825>
    {
        public LoginPingCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0x0825(data, _user);
            _event_args = new QQEventArgs<Receive_0x0825>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}