using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    [ReceivePacketCommand(QQCommand.Data0X01C5)]
    public class Data0X01C5Command : ReceiveCommand<Receive_0X01C5>
    {
        public Data0X01C5Command(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X01C5(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X01C5>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}