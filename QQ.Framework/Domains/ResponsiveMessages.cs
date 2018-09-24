using QQ.Framework.Utils;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     可响应的消息事件列表
    /// </summary>
    public interface IResponsiveMessages
    {
        /// <summary>
        ///     收到好友消息
        /// </summary>
        /// <param name="friendNumber">好友qq</param>
        /// <param name="content">内容</param>
        void ReceiveFriendMessage(long friendNumber, Richtext content);

        /// <summary>
        ///     收到群组消息
        /// </summary>
        /// <param name="groupNumber">QQ群号</param>
        /// <param name="fromNumber">发言者QQ</param>
        /// <param name="content">内容</param>
        void ReceiveGroupMessage(long groupNumber, long fromNumber, Richtext content);
    }
}