using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     撤回群消息
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X03F7)]
    public class RecallGroupMessagesCommand : ReceiveCommand<Receive_0X03F7>
    {
        public RecallGroupMessagesCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0X03F7(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X03F7>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}