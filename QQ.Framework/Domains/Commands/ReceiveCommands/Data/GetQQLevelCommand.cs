using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     获取QQ等级
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0x005C)]
    public class GetQQLevelCommand : ReceiveCommand<Receive_0x005C>
    {
        /// <summary>
        ///     获取QQ等级
        /// </summary>
        public GetQQLevelCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0x005C(data, _user);
            _event_args = new QQEventArgs<Receive_0x005C>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}