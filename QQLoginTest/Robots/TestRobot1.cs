using System;
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Utils;

namespace QQLoginTest.Robots
{
    public class TestRobot : CustomRobot
    {
        public TestRobot(ISendMessageService service, IServerMessageSubject transponder, QQUser user) : base(service, transponder, user)
        {
        }

        public override void ReceiveFriendMessage(long friendNumber, Richtext content)
        {
            Console.WriteLine($"机器人收到来自{friendNumber}的消息{content}");
            if (content.ToString().StartsWith("~"))
            {
                _service.SendToFriend(friendNumber, content);
            }
        }

        public override void ReceiveGroupMessage(long groupNumber, long fromNumber, Richtext content)
        {
            Console.WriteLine($"机器人收到来自{groupNumber}的{fromNumber}的消息{content}");
            if (content.ToString().StartsWith("~"))
            {
                _service.SendToGroup(groupNumber, content);
            }
        }
    }
}
