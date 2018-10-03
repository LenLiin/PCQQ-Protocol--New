using System;
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Utils;

namespace QQ.FrameworkTest.Robots
{
    public class TestRobot : CustomRobot
    {
        public TestRobot(ISendMessageService service, IServerMessageSubject transponder, QQUser user) : base(service, transponder, user)
        {
        }

        public override void ReceiveFriendMessage(long friendNumber, Richtext content)
        {
            Console.WriteLine($"机器人收到来自{friendNumber}的消息{content}");
        }

        public override void ReceiveGroupMessage(long groupNumber, long fromNumber, Richtext content)
        {
        }
    }
}
