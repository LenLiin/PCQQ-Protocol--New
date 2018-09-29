using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     收到群/系统消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X0017)]
    public class ReceiveGroupSystemMessagesCommand : ReceiveCommand<Receive_0X0017>
    {
        /// <summary>
        ///     收到群/系统消息
        /// </summary>
        public ReceiveGroupSystemMessagesCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0X0017(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0017>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();

            // 将收到的群/系统消息转发给所有机器人
            _transponder.ReceiveGroupMessage(_packet.Group, _packet.FromQQ, _packet.Message);
        }
    }
}