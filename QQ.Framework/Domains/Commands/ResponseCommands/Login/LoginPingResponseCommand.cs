using System;
using System.Collections.Generic;
using System.Text;
using QQ.Framework.Packets;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0x0825)]
    public class LoginPingResponseCommand : ResponseCommand<Receive_0x0825>
    {
        public LoginPingResponseCommand(QQEventArgs<Receive_0x0825> args) : base(args)
        {
        }

        public override void Process()
        {
            var packet = _args.ReceivePacket;
            var client = _args.QQClient;
            var user = _args.QQClient.QQUser;

            if (packet.DataHead == 0xFE)
            {
                client.MessageLog($"服务器{Util.GetIpStringFromBytes(user.ServerIp)}重定向");
                //如果是登陆重定向，继续登陆
                user.IsLoginRedirect = true;
                client.LoginServerHost = Util.GetIpStringFromBytes(user.ServerIp);
                //刷新重定向后服务器IP
                client.RefPoint();
                //重新发送登录Ping包
                client.Send(new Send_0x0825(user, true).WriteData());
            }
            else
            {
                client.MessageLog($"连接服务器{Util.GetIpStringFromBytes(user.ServerIp)}成功");
                //Ping请求成功后发送0836登录包
                client.Send(new Send_0x0836(user, Login0x0836Type.Login0x0836_622).WriteData());
            }
        }
    }
}
