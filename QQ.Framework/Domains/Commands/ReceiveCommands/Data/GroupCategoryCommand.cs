using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     群分组信息查询
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0X0195)]
    public class GroupCategoryCommand : ReceiveCommand<Receive_0X0195>
    {
        /// <summary>
        ///     群分组信息查询
        /// </summary>
        public GroupCategoryCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0195(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0195>(_service, _user, _packet);
        }

        public override void Process()
        {
        }
    }
}