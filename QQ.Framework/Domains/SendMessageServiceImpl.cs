using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Utils;

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
            _user.FriendMessages.Add(message);//添加到消息列表
        }

        public void SendToGroup(long groupNumber, Richtext content)
        {
            var message = new Send_0X0002(_user, content, groupNumber);
            _socketService.Send(message);
            foreach (var packet in message.Following)
            {
                _socketService.Send(packet);
            }
            _user.GroupMessages.Add(message);//添加到消息列表
        }
    }
}