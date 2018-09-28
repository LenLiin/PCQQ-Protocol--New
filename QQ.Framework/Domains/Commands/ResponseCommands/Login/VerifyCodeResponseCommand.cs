using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0X00Ba)]
    public class VerifyCodeResponseCommand : ResponseCommand<Receive_0X00Ba>
    {
        public VerifyCodeResponseCommand(QQEventArgs<Receive_0X00Ba> args) : base(args)
        {
        }

        public override void Process()
        {
            if (_packet.Status == 0x01)
            {
                if (_packet.VerifyCommand == 0x01)
                {
                    _service.Send(new Send_0X00Ba(_user, ""));
                }
            }
            else
            {
                _user.QQPacket00BaSequence = 0x00;
                _user.QQPacketTgtgtKey = Util.RandomKey();
                //验证码验证成功后发送0836登录包
                _service.Send(new Send_0X0836(_user, Login0X0836Type.Login0X0836686, true));
            }
        }
    }
}