using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{
    public class GroupMembers
    {
        public int? Ec { get; set; }
        public int? AdmMax { get; set; }
        public int? AdmNum { get; set; }
        public int? Count { get; set; }
        public int? MaxCount { get; set; }
        public int? SearchCount { get; set; }
        public long? SvrTime { get; set; }
        public int? Vecsize { get; set; }
        public List<LevelName> Levelname { get; set; }
        public List<GroupMember> Mems { get; set; }
    }

    public class LevelName
    {
        public int? Level { get; set; }
        public string Name { get; set; }
    }

    public class GroupMember
    {
        public string Card { get; set; }
        public int? Flag { get; set; }
        public int? G { get; set; }
        public long? JoinTime { get; set; }
        public long? LastSpeakTime { get; set; }
        public QQLevel Lv { get; set; }
        public string Nick { get; set; }
        public int? Qage { get; set; }
        public int? Role { get; set; }
        public int? Tags { get; set; }
        public long? Uin { get; set; }
    }

    public class QQLevel
    {
        public int? Level { get; set; }
        public int? Point { get; set; }
    }
}