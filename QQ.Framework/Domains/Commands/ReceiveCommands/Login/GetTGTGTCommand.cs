using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0836)]
    public class GetTGTGTCommand : ReceiveCommand<Receive_0x0836>
    {
        public GetTGTGTCommand(byte[] data, SocketService service, ServerMessageSubject transponder, QQUser user) :
            base(data, service, transponder, user)
        {
            _packet = new Receive_0x0836(data, _user);
            _event_args = new QQEventArgs<Receive_0x0836>(_service, _user, _packet);
        }

        public override void Process()
        {
            if (_packet.Result != (byte)ResultCode.成功)
            {
                _service.MessageLog(_packet.ErrorMsg);
            }
            Response();
        }
    }
}