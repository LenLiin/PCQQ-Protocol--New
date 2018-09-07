using System.IO;
using System.Linq;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    /// <summary>
    ///     收到好友消息，回复已接收成功
    /// </summary>
    [ResponsePacketCommand(QQCommand.Message0x00CE)]
    public class ResponseReceiveFriendMessageCommand : ResponseCommand<Receive_0x00CE>
    {
        public ResponseReceiveFriendMessageCommand(QQEventArgs<Receive_0x00CE> args) : base(args)
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
                    user.MessageLog($"收到好友{packet.FromQQ}的乱码消息。");
                    //return;
                }

                user.MessageLog($"收到好友{packet.FromQQ}的消息:{packet.Message}");
            }
            else
            {
                user.MessageLog($"收到好友{packet.FromQQ}的空消息。");
            }

            var dataReader = new BinaryReader(new MemoryStream(packet.bodyDecrypted));

            client.Send(new Send_0x00CE(user, dataReader.ReadBytes(0x10), packet.Sequence).WriteData());

            //查看消息确认
            client.Send(new Send_0x0319(user, packet.FromQQ, packet.MessageDateTime).WriteData());
        }
    }
}