using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     收到QQ好友消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X00Ce)]
    public class ReceiveQQFriendMessagesCommand : ReceiveCommand<Receive_0X00Ce>
    {
        public ReceiveQQFriendMessagesCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0X00Ce(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X00Ce>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();

            // 将收到的好友消息转发给所有机器人
            _transponder.ReceiveFriendMessage(_packet.FromQQ, _packet.Message);
        }
    }
}