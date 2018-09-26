using System;
using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{

    public class GroupList
    {
        public int? ec { get; set; }
        /// <summary>
        /// 我创建的
        /// </summary>
        public List<GroupItem> create { get; set; }
        /// <summary>
        /// 我加入的
        /// </summary>
        public List<GroupItem> join { get; set; }
        /// <summary>
        /// 我管理的
        /// </summary>
        public List<GroupItem> manage { get; set; }
    }
    public class GroupItem
    {
        /// <summary>
        /// 群号
        /// </summary>
        public long? gc { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        public string gn { get; set; }
        /// <summary>
        /// 群主
        /// </summary>
        public long? owner { get; set; }
        public GroupMembers Members { get; set; }
    }
}