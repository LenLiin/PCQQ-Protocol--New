using System.Collections.Generic;
using QQ.Framework.Domains.Observers;
using QQ.Framework.Utils;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     消息转发器
    /// </summary>
    public class Transponder : IServerMessageSubject
    {
        /// <summary>
        ///     机器人列表
        /// </summary>
        private readonly IList<IServerMessageObserver> _robots;

        public Transponder()
        {
            _robots = new List<IServerMessageObserver>();
        }

        public void ReceiveFriendMessage(long friendNumber, Richtext content)
        {
            foreach (var robot in _robots)
            {
                robot.ReceiveFriendMessage(friendNumber, content);
            }
        }

        public void ReceiveGroupMessage(long groupNumber, long fromNumber, Richtext content)
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
        public void AddCustomRoBot(IServerMessageObserver robot)
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
        public void RemoveCustomRoBot(IServerMessageObserver robot)
        {
            if (_robots.Contains(robot))
            {
                _robots.Remove(robot);
            }
        }
    }
}