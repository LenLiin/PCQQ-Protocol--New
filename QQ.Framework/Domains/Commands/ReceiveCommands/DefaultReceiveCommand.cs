using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands
{
    public class DefaultReceiveCommand : ReceiveCommand<ReceiveCurrency>
    {
        public DefaultReceiveCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user)
            : base(data, service, transponder, user)
        {
            _packet = new ReceiveCurrency(data, _user);
            _eventArgs = new QQEventArgs<ReceiveCurrency>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}