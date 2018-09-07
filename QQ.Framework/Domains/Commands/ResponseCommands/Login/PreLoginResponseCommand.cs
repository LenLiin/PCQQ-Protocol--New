using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Sockets;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Login
{
    [ResponsePacketCommand(QQCommand.Login0x0828)]
    public class PreLoginResponseCommand : ResponseCommand<Receive_0x0828>
    {
        public PreLoginResponseCommand(QQEventArgs<Receive_0x0828> args) : base(args)
        {
        }

        public override void Process()
        {
            var client = _args.QQClient;
            var user = client.QQUser;

            client.MessageLog($"获取SessionKey");
            //二次发送0836登录包
            client.Send(new Send_0x00EC(user, LoginStatus.我在线上).WriteData());

            //定时发送心跳包
            var timersInvoke = new TimersInvoke(client);
            timersInvoke.StartTimer();
        }
    }
}