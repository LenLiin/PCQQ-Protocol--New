using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     发送QQ消息收到的返回包
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X00Cd)]
    public class SendingQQMessagesCommand : ReceiveCommand<Receive_0X00Cd>
    {
        public SendingQQMessagesCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) : base(data, service, transponder, user)
        {
            _packet = new Receive_0X00Cd(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X00Cd>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}