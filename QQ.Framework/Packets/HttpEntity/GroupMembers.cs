using System;
using System.Collections.Generic;

namespace QQ.Framework.HttpEntity
{

    public class GroupMembers
    {
        public int? ec { get; set; }
        public int? adm_max { get; set; }
        public int? adm_num { get; set; }
        public int? count { get; set; }
        public int? max_count { get; set; }
        public int? search_count { get; set; }
        public long? svr_time { get; set; }
        public int? vecsize { get; set; }
        public List<LevelName> levelname { get; set; }
        public List<GroupMember> mems { get; set; }
    }
    public class LevelName
    {
        public int? level { get; set; }
        public string name { get; set; }
    }
    public class GroupMember
    {
        public string card { get; set; }
        public int? flag { get; set; }
        public int? g { get; set; }
        public long? join_time { get; set; }
        public long? last_speak_time { get; set; }
        public Level lv { get; set; }
        public string nick { get; set; }
        public int? qage { get; set; }
        public int? role { get; set; }
        public int? tags { get; set; }
        public long? uin { get; set; }
    }
    public class Level
    {
        public int? level { get; set; }
        public int? point { get; set; }
    }
}
