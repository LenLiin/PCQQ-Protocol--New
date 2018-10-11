using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     撤回QQ好友消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X03FC)]
    public class RecallQQFriendMessagesCommand : ReceiveCommand<Receive_0X03FC>
    {
        public RecallQQFriendMessagesCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0X03FC(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X03FC>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}