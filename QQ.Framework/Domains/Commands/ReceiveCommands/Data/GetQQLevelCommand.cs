using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     获取QQ等级
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0X005C)]
    public class GetQQLevelCommand : ReceiveCommand<Receive_0X005C>
    {
        /// <summary>
        ///     获取QQ等级
        /// </summary>
        public GetQQLevelCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X005C(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X005C>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}