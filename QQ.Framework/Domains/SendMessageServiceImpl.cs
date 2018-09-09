using QQ.Framework.Packets.Send.Login;
using System;
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
            _socketService.Send(new Send_0x00CD(_user, content, MessageType.Normal, friendNumber));
        }

        public void SendToGroup(long groupNumber, Richtext content)
        {
            _socketService.Send(new Send_0x0002(_user, content, MessageType.Normal, groupNumber));
        }
    }
}