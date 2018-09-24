using QQ.Framework.Domains.Observers;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     服务端主题, 用来转发可处理的消息
    /// </summary>
    public interface IServerMessageSubject : IResponsiveMessages
    {
        /// <summary>
        ///     将自定义机器人加入观察者列表。
        /// </summary>
        /// <param name="robot"></param>
        void AddCustomRoBot(IServerMessageObserver robot);

        /// <summary>
        ///     将自定义机器人移除观察者列表。
        ///     移除后将不能响应收到的信息。
        /// </summary>
        /// <param name="robot"></param>
        void RemoveCustomRoBot(IServerMessageObserver robot);
    }
}