using System;
using System.Collections.Generic;
using System.Text;

namespace QQ.Framework.Domains.Observers
{
    /// <summary>
    /// 响应接收到的消息。
    /// 可响应的事件均定义在此处,实现个性化机器人需实现此类
    /// </summary>
    public interface ServerMessageObserver
    {
        /// <summary>
        /// 收到好友消息
        /// </summary>
        void ReceiveFriendMessage();

        /// <summary>
        /// 收到群组消息
        /// </summary>
        void ReceiveGroupMessage();
    }
}
