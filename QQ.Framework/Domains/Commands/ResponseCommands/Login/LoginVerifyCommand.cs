using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0X0836)]
    public class LoginVerifyCommand : ResponseCommand<Receive_0X0836>
    {
        public LoginVerifyCommand(QQEventArgs<Receive_0X0836> args) : base(args)
        {
        }

        public override void Process()
        {
            var packetLength = _packet.GetPacketLength();

            if (_packet.Result == (byte) ResultCode.需要更新TGTGT
                || _packet.Result == (byte) ResultCode.需要重新CheckTGTGT)
            {
                _service.MessageLog("二次登录");
                _service.Send(new Send_0X0836(_user, Login0X0836Type.Login0X0836686));
            }
            else if (_packet.Result == (byte) ResultCode.需要验证码)
            {
                _service.Send(new Send_0X00Ba(_user, ""));
            }
            else if (_packet.Result == (byte) ResultCode.成功)
            {
                //_service.Send(new Send_0X0839(_user));
                _service.MessageLog("登陆成功获取个人基本信息");
                _service.MessageLog($"账号：{_user.QQ}，昵称：{_user.NickName}，年龄：{_user.Age}，性别：{_user.Gender}");
                _service.Send(new Send_0X0828(_user));
            }
        }
    }
}