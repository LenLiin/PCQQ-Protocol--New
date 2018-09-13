using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains
{
    public class SendMessageServiceImpl : SendMessageService
    {
        private readonly SocketService _socketService;
        private readonly QQUser _user;

        public SendMessageServiceImpl(SocketService socketService, QQUser user)
        {
            _socketService = socketService;
            _user = user;
        }

        public void SendToFriend(long friendNumber, Richtext content)
        {
            foreach (var data in Send_0x00CD.ConstructMessage(_user, content, friendNumber))
            {
                _socketService.Send(new Send_0x00CD(_user, data));
            }
        }

        public void SendToGroup(long groupNumber, Richtext content)
        {
            foreach (var data in Send_0x0002.ConstructMessage(_user, content, groupNumber))
            {
                _socketService.Send(new Send_0x0002(_user, data));
            }
        }
    }
}