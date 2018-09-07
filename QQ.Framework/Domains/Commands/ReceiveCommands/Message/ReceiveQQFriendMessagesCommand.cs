using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     收到QQ好友消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x00CE)]
    public class ReceiveQQFriendMessagesCommand : ReceiveCommand<Receive_0x00CE>
    {
        public ReceiveQQFriendMessagesCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x00CE(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x00CE>(client, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}