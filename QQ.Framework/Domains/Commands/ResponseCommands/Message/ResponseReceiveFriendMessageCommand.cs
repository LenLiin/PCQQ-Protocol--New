using System.IO;
using System.Linq;
using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    /// <summary>
    ///     收到好友消息，回复已接收成功
    /// </summary>
    [ResponsePacketCommand(QQCommand.Message0X00Ce)]
    public class ResponseReceiveFriendMessageCommand : ResponseCommand<Receive_0X00Ce>
    {
        public ResponseReceiveFriendMessageCommand(QQEventArgs<Receive_0X00Ce> args) : base(args)
        {
        }

        public override void Process()
        {
            if (!string.IsNullOrEmpty(_packet.Message))
            {
                if (!QQGlobal.DebugLog && _packet.Message.ToString().Count(c => c == '\0') > 5)
                {
                    _service.MessageLog($"收到好友{_packet.FromQQ}的乱码消息。");
                    //return;
                }

                _service.MessageLog($"收到好友{_packet.FromQQ}的消息:{_packet.Message}");
            }
            else
            {
                _service.MessageLog($"收到好友{_packet.FromQQ}的空消息。");
            }

            var dataReader = new BinaryReader(new MemoryStream(_packet.BodyDecrypted));

            _service.Send(new Send_0X00Ce(_user, dataReader.ReadBytes(0x10), _packet.Sequence));

            //查看消息确认
            _service.Send(new Send_0X0319(_user, _packet.FromQQ, _packet.MessageDateTime));
        }
    }
}