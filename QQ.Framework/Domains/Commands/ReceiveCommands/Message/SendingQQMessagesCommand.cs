using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     发送QQ消息收到的返回包
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x00CD)]
    public class SendingQQMessagesCommand : ReceiveCommand<Receive_0x00CD>
    {
        public SendingQQMessagesCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x00CD(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x00CD>(client, _packet);
        }

        public override void Process()
        {
        }
    }
}