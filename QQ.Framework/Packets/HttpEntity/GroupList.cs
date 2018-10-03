using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{
    public class GroupList
    {
        public int? Ec { get; set; }

        /// <summary>
        ///     我创建的
        /// </summary>
        public List<GroupItem> Create { get; set; }

        /// <summary>
        ///     我加入的
        /// </summary>
        public List<GroupItem> Join { get; set; }

        /// <summary>
        ///     我管理的
        /// </summary>
        public List<GroupItem> Manage { get; set; }
    }

    public class GroupItem
    {
        /// <summary>
        ///     群号
        /// </summary>
        public long? Gc { get; set; }

        /// <summary>
        ///     群名称
        /// </summary>
        public string Gn { get; set; }

        /// <summary>
        ///     群主
        /// </summary>
        public long? Owner { get; set; }

        public GroupMembers Members { get; set; }
    }
}