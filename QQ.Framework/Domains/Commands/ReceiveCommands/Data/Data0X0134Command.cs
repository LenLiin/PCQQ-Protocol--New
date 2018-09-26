using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    [ReceivePacketCommand(QQCommand.Data0X0134)]
    public class Data0X0134Command : ReceiveCommand<Receive_0X0134>
    {
        public Data0X0134Command(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0134(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0134>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}