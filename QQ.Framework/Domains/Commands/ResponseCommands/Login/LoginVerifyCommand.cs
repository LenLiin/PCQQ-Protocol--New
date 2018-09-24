using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0x0836)]
    public class LoginVerifyCommand : ResponseCommand<Receive_0x0836>
    {
        public LoginVerifyCommand(QQEventArgs<Receive_0x0836> args) : base(args)
        {
        }

        public override void Process()
        {
            var packet_length = _packet.GetPacketLength();

            if (_packet.Result == (byte)ResultCode.需要更新TGTGT
                || _packet.Result == (byte)ResultCode.需要重新CheckTGTGT)
            {
                _service.MessageLog("二次登录");
                _service.Send(new Send_0x0836(_user, Login0x0836Type.Login0x0836_686));
            }
            else if (_packet.Result == (byte)ResultCode.需要验证码)
            {
                _service.Send(new Send_0x00BA(_user, ""));
            }
            else if (_packet.Result == (byte)ResultCode.成功)
            {
                _service.MessageLog("登陆成功获取个人基本信息");
                _service.MessageLog($"账号：{_user.QQ}，昵称：{_user.NickName}，年龄：{_user.Age}，性别：{_user.Gender}");
                _service.Send(new Send_0x0828(_user));
            }
        }
    }
}