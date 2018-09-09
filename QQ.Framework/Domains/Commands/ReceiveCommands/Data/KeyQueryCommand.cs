using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     Key查询
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0x001D)]
    public class KeyQueryCommand : ReceiveCommand<Receive_0x001D>
    {
        /// <summary>
        ///     Key查询
        /// </summary>
        public KeyQueryCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x001D(data, _user);
            _event_args = new QQEventArgs<Receive_0x001D>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}