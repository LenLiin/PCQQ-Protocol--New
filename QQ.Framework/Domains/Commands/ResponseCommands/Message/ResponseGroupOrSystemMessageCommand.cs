using System.IO;
using System.Linq;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    [ResponsePacketCommand(QQCommand.Message0x0017)]
    public class ResponseGroupOrSystemMessageCommand : ResponseCommand<Receive_0x0017>
    {
        public ResponseGroupOrSystemMessageCommand(QQEventArgs<Receive_0x0017> args) : base(args)
        {
        }

        public override void Process()
        {
            var client = _args.QQClient;
            var user = client.QQUser;
            var packet = _args.ReceivePacket;

            if (!string.IsNullOrEmpty(packet.Message))
            {
                if (!QQGlobal.DebugLog && packet.Message.Count(c => c == '\0') > 5)
                {
                    user.MessageLog($"收到群{packet.Group}的{packet.FromQQ}的乱码消息。");
                }

                user.MessageLog($"收到群{packet.Group}的{packet.FromQQ}的消息:{packet.Message}");
            }
            else
            {
                user.MessageLog($"收到群{packet.Group}的{packet.FromQQ}的空消息。");
            }

            //提取数据
            var dataReader = new BinaryReader(new MemoryStream(packet.bodyDecrypted));

            client.Send(new Send_0x0017(user, dataReader.ReadBytes(0x10), packet.Sequence).WriteData());


            //查看群消息确认
            if (packet.ReceiveTime != null)
            {
                //TODO:未实现
                //client.Send(new Send_0x0360(user, packet.Group, packet.ReceiveTime).WriteData());
            }
        }
    }
}