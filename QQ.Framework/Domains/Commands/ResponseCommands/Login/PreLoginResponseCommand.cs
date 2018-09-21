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
            _service.MessageLog("获取SessionKey");
            //登录成功改变在线状态
            _service.Send(new Send_0x00EC(_user, LoginStatus.我在线上));

            //定时发送心跳包
            var timersInvoke = new TimersInvoke(_service, _user);
            timersInvoke.StartTimer();
        }
    }
}