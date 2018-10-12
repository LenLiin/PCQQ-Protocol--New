using System.IO;
using System.Linq;
using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    [ResponsePacketCommand(QQCommand.Message0X0017)]
    public class ResponseGroupOrSystemMessageCommand : ResponseCommand<Receive_0X0017>
    {
        public ResponseGroupOrSystemMessageCommand(QQEventArgs<Receive_0X0017> args) : base(args)
        {
        }

        public override void Process()
        {
            if (!string.IsNullOrEmpty(_packet.Message))
            {
                //当收到消息包序为自己发送的消息包序时更新消息Id
                if (_user.GroupSendMessages.Any(c => c.Sequence == _packet.Sequence))
                {
                    var messageSend = _user.GroupSendMessages
                        .FirstOrDefault(c => c.Sequence == _packet.Sequence);
                    if (messageSend != null)
                    {
                        messageSend.MessageId = _packet.MessageId;
                        messageSend.MessageIndex = _packet.MessageIndex;
                    }
                }
                else
                {
                    if (_user.GroupReceiveMessages.All(c => c.Sequence != _packet.Sequence))
                    {
                        if (!QQGlobal.DebugLog && _packet.Message.ToString().Count(c => c == '\0') > 5)
                        {
                            _service.MessageLog($"收到群{_packet.Group}的{_packet.FromQQ}的乱码消息。");
                        }

                        _service.MessageLog($"收到群{_packet.Group}的{_packet.FromQQ}的消息:{_packet.Message}");

                        //添加到已处理消息列表
                        _user.GroupReceiveMessages.Add(_packet);
                    }

                    //查看群消息确认
                    if (_packet.ReceiveTime != null)
                    {
                        _service.Send(new Send_0X0391(_user, _packet.Group, _packet.MessageIndex));
                    }
                }
            }

            //提取数据
            var dataReader = new BinaryReader(new MemoryStream(_packet.BodyDecrypted));

            _service.Send(new Send_0X0017(_user, dataReader.ReadBytes(0x10), _packet.Sequence));
        }
    }
}