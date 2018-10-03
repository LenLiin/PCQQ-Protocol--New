using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    [ReceivePacketCommand(QQCommand.Data0X019B)]
    public class Data0X019BCommand : ReceiveCommand<Receive_0X019B>
    {
        public Data0X019BCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X019B(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X019B>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}