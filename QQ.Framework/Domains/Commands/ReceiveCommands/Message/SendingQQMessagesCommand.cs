using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     发送QQ消息收到的返回包
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x00CD)]
    public class SendingQQMessagesCommand : ReceiveCommand<Receive_0x00CD>
    {
        public SendingQQMessagesCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0x00CD(data, _user);
            _event_args = new QQEventArgs<Receive_0x00CD>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}