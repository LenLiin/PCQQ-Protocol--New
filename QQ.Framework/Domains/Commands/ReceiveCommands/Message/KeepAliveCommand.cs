using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     KeepAlive（心跳）
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x0058)]
    public class KeepAliveCommand : ReceiveCommand<Receive_0x0058>
    {
        /// <summary>
        ///     KeepAlive（心跳）
        /// </summary>
        public KeepAliveCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x0058(data, _user);
            _event_args = new QQEventArgs<Receive_0x0058>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}