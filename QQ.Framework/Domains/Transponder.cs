using System;
using System.Collections.Generic;
using QQ.Framework.Domains.Observers;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     消息转发器
    /// </summary>
    public class Transponder : ServerMessageSubject
    {
        /// <summary>
        ///     机器人列表
        /// </summary>
        private readonly IList<ServerMessageObserver> observers;

        public Transponder()
        {
            observers = new List<ServerMessageObserver>();
        }

        public void ReceiveFriendMessage(long friendNumber, string content)
        {
            throw new NotImplementedException();
        }

        public void ReceiveGroupMessage(long groupNumber, long fromNumber, string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     添加机器人
        /// </summary>
        /// <param name="robot"></param>
        public void AddCustomRoBot(ServerMessageObserver robot)
        {
            if (!observers.Contains(robot))
            {
                observers.Add(robot);
            }
        }

        /// <summary>
        ///     移除机器人
        /// </summary>
        /// <param name="robot"></param>
        public void RemoveCustomRoBot(ServerMessageObserver robot)
        {
            if (observers.Contains(robot))
            {
                observers.Remove(robot);
            }
        }
    }
}