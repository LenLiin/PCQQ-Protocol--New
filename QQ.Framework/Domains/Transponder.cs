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
        private readonly IList<ServerMessageObserver> _robots;

        public Transponder()
        {
            _robots = new List<ServerMessageObserver>();
        }

        public void ReceiveFriendMessage(long friendNumber, string content)
        {
            foreach (var robot in _robots)
            {
                robot.ReceiveFriendMessage(friendNumber, content);
            }
        }

        public void ReceiveGroupMessage(long groupNumber, long fromNumber, string content)
        {
            foreach (var robot in _robots)
            {
                robot.ReceiveGroupMessage(groupNumber, fromNumber, content);
            }
        }

        /// <summary>
        ///     添加机器人
        /// </summary>
        /// <param name="robot"></param>
        public void AddCustomRoBot(ServerMessageObserver robot)
        {
            if (!_robots.Contains(robot))
            {
                _robots.Add(robot);
            }
        }

        /// <summary>
        ///     移除机器人
        /// </summary>
        /// <param name="robot"></param>
        public void RemoveCustomRoBot(ServerMessageObserver robot)
        {
            if (_robots.Contains(robot))
            {
                _robots.Remove(robot);
            }
        }
    }
}