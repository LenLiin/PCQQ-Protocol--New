using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0x00BA)]
    public class VerifyCodeResponseCommand : ResponseCommand<Receive_0x00BA>
    {
        public VerifyCodeResponseCommand(QQEventArgs<Receive_0x00BA> args) : base(args)
        {
        }

        public override void Process()
        {
            if (_packet.Status == 0x01)
            {
                if (_packet.VerifyCommand == 0x01)
                {
                    _service.Send(new Send_0x00BA(_user, ""));
                }
            }
            else
            {
                _user.QQ_0836Token = _user.QQ_PACKET_00BAVerifyToken;
                _user.QQ_PACKET_00BASequence = 0x00;
                _user.QQ_PACKET_TgtgtKey = Util.RandomKey();
                //验证码验证成功后发送0836登录包
                _service.Send(new Send_0x0836(_user, Login0x0836Type.Login0x0836_686, true));
            }
        }
    }
}