namespace QQ.Framework.Domains
{
    /// <summary>
    /// 发送消息服务
    /// </summary>
    public interface SendMessageServer
    {
        /// <summary>
        /// 发送给好友
        /// </summary>
        /// <param name="friendNumber">好友QQ</param>
        /// <param name="content">内容</param>
        void SendToFriend(long friendNumber, string content);

        /// <summary>
        /// 发送群消息
        /// </summary>
        /// <param name="groupNumber">QQ群号</param>
        /// <param name="content">内容</param>
        void SendToGroup(long groupNumber, string content);

        /// <summary>
        /// 发送群消息时@某些人
        /// </summary>
        /// <param name="groupNumber">QQ群号</param>
        /// <param name="content">内容</param>
        /// <param name="atList">@列表</param>
        void SendToGroupWithAt(long groupNumber, string content, params long[] atList);
    }
}