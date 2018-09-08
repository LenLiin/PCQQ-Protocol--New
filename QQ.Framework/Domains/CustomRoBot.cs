using QQ.Framework.Domains.Observers;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     自定义机器人基类
    /// </summary>
    public abstract class CustomRoBot : ServerMessageObserver
    {
        /// <summary>
        ///     消息发送服务
        /// </summary>
        protected readonly SendMessageService Service;

        /// <summary>
        ///     消息转发器
        /// </summary>
        private readonly ServerMessageSubject _transponder;

        public CustomRoBot(SendMessageService service, ServerMessageSubject transponder)
        {
            Service = service;
            _transponder = transponder;

            // 将机器人加入转发器的订阅列表中
            _transponder.AddCustomRoBot(this);
        }

        public abstract void ReceiveFriendMessage(long friendNumber, string content);
        public abstract void ReceiveGroupMessage(long groupNumber, long fromNumber, string content);
    }
}