using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     收到群/系统消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x0017)]
    public class ReceiveGroupSystemMessagesCommand : ReceiveCommand<Receive_0x0017>
    {
        /// <summary>
        ///     收到群/系统消息
        /// </summary>
        public ReceiveGroupSystemMessagesCommand(byte[] data, SocketService service, ServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x0017(data, _user);
            _event_args = new QQEventArgs<Receive_0x0017>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}