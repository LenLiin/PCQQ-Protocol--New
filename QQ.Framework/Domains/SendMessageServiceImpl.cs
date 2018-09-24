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
            _socketService.Send(new Send_0X00Cd(_user, content, MessageType.Normal, friendNumber));
        }

        public void SendToGroup(long groupNumber, Richtext content)
        {
            _socketService.Send(new Send_0X0002(_user, content, MessageType.Normal, groupNumber));
        }
    }
}