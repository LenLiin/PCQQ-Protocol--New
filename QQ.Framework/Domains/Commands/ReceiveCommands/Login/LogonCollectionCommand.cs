using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0X0839)]
    public class LogonCollection : ReceiveCommand<Receive_0X0839>
    {
        public LogonCollection(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0839(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0839>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}