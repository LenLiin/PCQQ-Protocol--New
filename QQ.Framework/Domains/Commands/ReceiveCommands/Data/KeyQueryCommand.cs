using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     Key查询
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0X001D)]
    public class KeyQueryCommand : ReceiveCommand<Receive_0X001D>
    {
        /// <summary>
        ///     Key查询
        /// </summary>
        public KeyQueryCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X001D(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X001D>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}