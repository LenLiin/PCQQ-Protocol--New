using QQ.Framework.Domains.Observers;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     自定义机器人基类
    /// </summary>
    public abstract class CustomRobot : IServerMessageObserver
    {
        /// <summary>
        ///     消息发送服务
        /// </summary>
        protected readonly ISendMessageService _service;

        /// <summary>
        ///     消息转发器
        /// </summary>
        private readonly IServerMessageSubject _transponder;

        /// <summary>
        ///     账号信息
        /// </summary>
        protected readonly QQUser _user;

        public CustomRobot(ISendMessageService service, IServerMessageSubject transponder, QQUser user)
        {
            _service = service;
            _transponder = transponder;
            _user = user;

            // 将机器人加入转发器的订阅列表中
            _transponder.AddCustomRoBot(this);
        }

        public abstract void ReceiveFriendMessage(long friendNumber, Richtext content);
        public abstract void ReceiveGroupMessage(long groupNumber, long fromNumber, Richtext content);
    }
}