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
            var packet = _args.ReceivePacket;
            var client = _args.QQClient;
            var user = client.QQUser;

            if (packet.Status == 0x01)
            {
                if (packet.VerifyCommand == 0x01)
                {
                    client.Send(new Send_0x00BA(user, "").WriteData());
                }
            }
            else
            {
                user.QQ_0836Token = user.QQ_PACKET_00BAVerifyToken;
                user.QQ_PACKET_00BASequence = 0x00;
                user.QQ_PACKET_TgtgtKey = Util.RandomKey();
                //验证码验证成功后发送0836登录包
                client.Send(new Send_0x0836(user, Login0x0836Type.Login0x0836_686, true).WriteData());
            }
        }
    }
}
