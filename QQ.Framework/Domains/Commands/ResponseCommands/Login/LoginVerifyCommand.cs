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
            var packet_length = _args.ReceivePacket.GetPacketLength();
            var user = _args.QQClient.QQUser;

            if (packet_length == 871 && _args.ReceivePacket.VerifyCommand == 0x01)
            {                
                _args.QQClient.Send(new Send_0x00BA(user, "").WriteData());
            }
            else if (packet_length > 700)
            {
                _args.QQClient.MessageLog($"登陆成功获取个人基本信息");
                _args.QQClient.MessageLog($"账号：{user.QQ}，昵称：{user.NickName}，年龄：{user.Age}，性别：{user.Gender}");
                _args.QQClient.Send(new Send_0x0828(user).WriteData());
            }
        }
    }
}
