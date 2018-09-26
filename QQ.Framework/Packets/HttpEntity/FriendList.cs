using System;
using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{
    public class FriendList
    {

        public int? ec { get; set; }
        public List<FriendCateItem> result { get; set; }
    }
    public class FriendCateItem
    {
        public string gname { get; set; }
        public List<FriendItem> mems { get; set; }
    }
    public class FriendItem
    {
        public string name { get; set; }
        public long? uin { get; set; }
    }
}
