using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x00BA)]
    public class VerifyCodeCommand : ReceiveCommand<Receive_0x00BA>
    {
        public VerifyCodeCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x00BA(data, _user);
            _event_args = new QQEventArgs<Receive_0x00BA>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();

            if (_packet.VerifyCommand == 0x02)
            {
                _service.ReceiveVerifyCode(_user.QQ_PACKET_00BAVerifyCode);
            }
        }
    }
}