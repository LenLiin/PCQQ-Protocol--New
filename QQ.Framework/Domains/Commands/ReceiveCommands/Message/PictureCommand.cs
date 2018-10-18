using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     图片处理（上传或获取）
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0X0388)]
    public class PictureCommand : ReceiveCommand<Receive_0X0388>
    {
        /// <summary>
        ///     图片处理（上传或获取）
        /// </summary>
        public PictureCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0388(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0388>(_service, _user, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}