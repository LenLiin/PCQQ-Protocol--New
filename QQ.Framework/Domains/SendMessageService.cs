using QQ.Framework.Utils;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     发送消息服务
    /// </summary>
    public interface ISendMessageService
    {
        /// <summary>
        ///     发送给好友
        /// </summary>
        /// <param name="friendNumber">好友QQ</param>
        /// <param name="content">内容</param>
        void SendToFriend(long friendNumber, Richtext content);

        /// <summary>
        ///     发送群消息
        /// </summary>
        /// <param name="groupNumber">QQ群号</param>
        /// <param name="content">内容</param>
        void SendToGroup(long groupNumber, Richtext content);
    }
}