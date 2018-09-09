using QQ.Framework.Packets.Send.Login;

namespace QQ.Framework.Domains
{
    public class SendMessageServiceImpl : SendMessageService
    {
        private readonly SocketService _socketService;

        public SendMessageServiceImpl(SocketService socketService)
        {
            _socketService = socketService;
        }

        public void SendToFriend(long friendNumber, string content)
        {
            throw new System.NotImplementedException();
        }

        public void SendToGroup(long groupNumber, string content)
        {
            throw new System.NotImplementedException();
        }

        public void SendToGroupWithAt(long groupNumber, string content, params long[] atList)
        {
            throw new System.NotImplementedException();
        }
    }
}