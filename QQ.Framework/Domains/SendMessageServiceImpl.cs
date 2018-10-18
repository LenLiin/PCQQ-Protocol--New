using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Utils;
using System;
using System.Linq;

namespace QQ.Framework.Domains
{
    public class SendMessageServiceImpl : ISendMessageService
    {
        private readonly ISocketService _socketService;
        private readonly QQUser _user;

        public SendMessageServiceImpl(ISocketService socketService, QQUser user)
        {
            _socketService = socketService;
            _user = user;
        }

        public void SendToFriend(long friendNumber, Richtext content)
        {
            var message = new Send_0X00Cd(_user, content, friendNumber);
            _socketService.Send(message);
            foreach (var packet in message.Following)
            {
                _socketService.Send(packet);
            }

            _user.FriendSendMessages.Add(message); //添加到消息列表
        }
        public void SendToGroup(long groupNumber, Richtext content)
        {
            //有图片的时候先不发消息
            var message = new Send_0X0002(_user, content, groupNumber);
            if (content.Snippets.Any(c => c.Type == MessageType.Picture))
            {
                var PictureSnippets = content.Snippets.Where(c => c.Type == MessageType.Picture).ToList();

                foreach (var pictureSnippet in PictureSnippets)
                {
                    //发送图片
                    var picture = new Send_0X0388(_user, pictureSnippet, groupNumber);
                    picture.Sequence = message.Sequence;
                    _socketService.Send(picture);
                }
            }
            else
            {
                _socketService.Send(message);
                foreach (var packet in message.Following)
                {
                    _socketService.Send(packet);
                }

            }
            _user.GroupSendMessages.Add(message);//添加到消息列表
        }
    }
}