using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands
{
    public class DefaultReceiveCommand : ReceiveCommand<Receive_Currency>
    {
        public DefaultReceiveCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user)
            : base(data, service, transponder, user)
        {
            _packet = new Receive_Currency(data, _user);
            _event_args = new QQEventArgs<Receive_Currency>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}