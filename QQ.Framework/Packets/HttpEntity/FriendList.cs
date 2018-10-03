using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{
    public class FriendList
    {
        public int? Ec { get; set; }
        public List<FriendCateItem> Result { get; set; }
    }

    public class FriendCateItem
    {
        public string Gname { get; set; }
        public List<FriendItem> Mems { get; set; }
    }

    public class FriendItem
    {
        public string Name { get; set; }
        public long? Uin { get; set; }
    }
}