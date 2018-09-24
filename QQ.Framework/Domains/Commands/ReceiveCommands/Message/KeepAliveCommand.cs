using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     KeepAlive（心跳）
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X0058)]
    public class KeepAliveCommand : ReceiveCommand<Receive_0X0058>
    {
        /// <summary>
        ///     KeepAlive（心跳）
        /// </summary>
        public KeepAliveCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0058(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0058>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}