using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0X0836)]
    public class GetTgtgtCommand : ReceiveCommand<Receive_0X0836>
    {
        public GetTgtgtCommand(byte[] data, ISocketService service, IServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0X0836(data, _user);
            _eventArgs = new QQEventArgs<Receive_0X0836>(_service, _user, _packet);
        }

        public override void Process()
        {
            if (_packet.Result != (byte) ResultCode.成功)
            {
		// _service.MessageLog(_packet.ErrorMsg);
		if(_packet.Result != (byte) ResultCode.需要更新TGTGT
		   && _packet.Result != (byte) ResultCode.需要重新CheckTGTGT) {
		    // 忽略二次登录
		    _service.LoginCallback(false, _packet.ErrorMsg);
		}
            }

            Response();
        }
    }
}
