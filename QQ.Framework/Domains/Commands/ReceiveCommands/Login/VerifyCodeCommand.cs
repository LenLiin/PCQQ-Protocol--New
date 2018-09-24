using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0X00Ba)]
    public class VerifyCodeCommand : ReceiveCommand<Receive_0X00Ba>
    {
        public VerifyCodeCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X00Ba(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X00Ba>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();

            if (_packet.VerifyCommand == 0x02)
            {
                _service.ReceiveVerifyCode(_user.QQPacket00BaVerifyCode);
            }
        }
    }
}