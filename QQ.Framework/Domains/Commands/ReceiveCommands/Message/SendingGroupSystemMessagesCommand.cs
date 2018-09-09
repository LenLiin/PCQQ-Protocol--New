using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     发送群消息后收到的回复包
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x0002)]
    public class SendingGroupSystemMessagesCommand : ReceiveCommand<Receive_0x0002>
    {
        public SendingGroupSystemMessagesCommand(byte[] data, SocketService service, ServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x0002(data, _user);
            _event_args = new QQEventArgs<Receive_0x0002>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}