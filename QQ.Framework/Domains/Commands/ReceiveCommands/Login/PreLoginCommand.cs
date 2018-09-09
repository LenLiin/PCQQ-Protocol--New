using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0828)]
    public class PreLoginCommand : ReceiveCommand<Receive_0x0828>
    {
        public PreLoginCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0x0828(data, _user);
            _event_args = new QQEventArgs<Receive_0x0828>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}