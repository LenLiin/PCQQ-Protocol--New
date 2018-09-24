using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0X00Ec)]
    public class OnLineStateCommand : ReceiveCommand<Receive_0X00Ec>
    {
        public OnLineStateCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X00Ec(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X00Ec>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}