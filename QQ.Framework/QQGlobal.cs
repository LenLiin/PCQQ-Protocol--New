namespace QQ.Framework
{
    /// <summary>
    ///     定义一些QQ用到的常量
    /// </summary>
    public static class QQGlobal
    {
        /// <summary>
        ///     包最大大小
        /// </summary>
        public const int QQPacketMaxSize = 65535;

        /// <summary>
        ///     QQ缺省编码方式
        /// </summary>
        public const string QQCharsetDefault = "UTF-8";

        /// <summary>
        ///     密钥长度
        /// </summary>
        public const int QQLengthKey = 16;

        /// <summary>
        ///     包起始标识
        /// </summary>
        public const byte QQHeaderBasicFamily = 0x02;

        /// <summary>
        ///     包结尾标识
        /// </summary>
        public const byte QQHeader03Family = 0x03;


        /// <summary>
        ///     基本协议族输入包的包头长度
        /// </summary>
        public const int QQLengthBasicFamilyInHeader = 7;

        /// <summary>
        ///     基本协议族输出包的包头长度
        /// </summary>
        public const int QQLengthBasicFamilyOutHeader = 11;

        /// <summary>
        ///     基本协议族包尾长度
        /// </summary>
        public const int QQLengthBasicFamilyTail = 1;

        /// <summary>
        ///     FTP协议族包头长度
        /// </summary>
        public const int QQLengthFtpFamilyHeader = 46;

        /// <summary>
        ///     05协议族包头长度
        /// </summary>
        public const int QQLength05FamilyHeader = 13;

        /// <summary>
        ///     05协议族包尾长度
        /// </summary>
        public const int QQLength05FamilyTail = 1;

        /// <summary>
        ///     网络硬盘协议族输入包包头长度
        /// </summary>
        public const int QQLengthDiskFamilyInHeader = 82;

        /// <summary>
        ///     网络硬盘协议族输出包包头长度
        /// </summary>
        public const int QQLengthDiskFamilyOutHeader = 154;

        /// <summary>
        ///     程序缺省使用的客户端版本号
        /// </summary>
        public const char QQClientVersion = QQClientVersion_0E1B;

        /// <summary>
        ///     客户端版本号标志 - TIM1.0
        /// </summary>
        public const char QQClientVersion_0E1B = (char) 0x3713;

        /// <summary>
        ///     QQ UDP缺省端口
        /// </summary>
        public const int QQPortUdp = 8000;

        /// <summary>
        ///     QQ TCP缺省端口
        /// </summary>
        public const int QQPortTcp = 443;

        /// <summary>
        ///     使用HTTP代理时连接QQ服务器的端口
        /// </summary>
        public const int QQPortHttp = 80;


        /// <summary>
        ///     QQ分组的名称最大字节长度，注意一个汉字是两个字节
        /// </summary>
        public const int QQMaxGroupName = 16;

        /// <summary>
        ///     QQ昵称的最长长度
        /// </summary>
        public const int QQMaxNameLength = 250;

        /// <summary>
        ///     QQ缺省表情个数
        /// </summary>
        public const int QQCountDefaultFace = 96;

        /// <summary>
        ///     得到用户信息的回复包字段个数
        /// </summary>
        public const int QQCountGetUserInfoField = 37;

        /// <summary>
        ///     修改用户信息的请求包字段个数，比实际的多1，最开始的QQ号不包括
        /// </summary>
        public const int QQCountModifyUserInfoField = 35;

        /// <summary>
        ///     用户备注信息的字段个数
        /// </summary>
        public const int QQCountRemarkField = 7;


        // 用户标志，比如QQFriend类，好友状态改变包都包含这样的标志
        /// <summary>
        ///     有摄像头
        /// </summary>
        public const int QQFlagCam = 0x80;


        /// <summary>
        ///     服务器端版本号 (不一定)
        ///     不一定真的是表示服务器端版本号，似乎和发出的包不同，这个有其他的含义，
        ///     感觉像是包的类型标志
        /// </summary>
        public const char QQServerVersion0100 = (char) 0x0100;

        /// <summary>
        ///     是否打开控制台日志
        /// </summary>
        public static bool DebugLog = true;

        public static byte[] QqexeMD5 { get; set; } =
            {0xfa, 0xcf, 0x7c, 0xc5, 0xae, 0x02, 0xe6, 0x65, 0x0c, 0x01, 0x07, 0xcd, 0xfe, 0x0e, 0x1b, 0x2c};
    }

    /// <summary>
    ///     性别
    /// </summary>
    public enum Gender : byte
    {
        /// <summary>
        ///     男
        /// </summary>
        GG = 0,

        /// <summary>
        ///     女
        /// </summary>
        MM = 1,

        /// <summary>
        ///     未知
        /// </summary>
        Unknown = (byte) 0xFF
    }

    /// <summary>
    ///     登录模式
    /// </summary>
    public enum LoginMode : byte
    {
        /// <summary>
        ///     正常
        /// </summary>
        Normal = 0x0A,

        /// <summary>
        ///     隐身
        /// </summary>
        Hidden = 0x28
    }

    /// <summary>
    ///     在线状态
    /// </summary>
    public enum QQStatus : byte
    {
        /// <summary>
        ///     在线
        /// </summary>
        Online = 0x0A,

        /// <summary>
        ///     离线
        /// </summary>
        Offline = 0x14,

        /// <summary>
        ///     离开
        /// </summary>
        Away = 0x1E,

        /// <summary>
        ///     隐身
        /// </summary>
        Hidden = 0x28
    }

    /// <summary>
    ///     认证类型，加一个人为好友时是否需要验证等等
    /// </summary>
    public enum AuthType : byte
    {
        /// <summary>
        ///     不需认证
        /// </summary>
        No = 0,

        /// <summary>
        ///     需要认证
        /// </summary>
        Need = 1,

        /// <summary>
        ///     对方拒绝加好友
        /// </summary>
        Reject = 2
    }

    /// <summary>
    ///     联系方法的可见类型
    /// </summary>
    public enum OpenContact
    {
        /// <summary>
        ///     完全公开
        /// </summary>
        Open = 0,

        /// <summary>
        ///     仅好友可见
        /// </summary>
        Friends = 1,

        /// <summary>
        ///     完全保密
        /// </summary>
        Close = 2
    }

    /// <summary>
    ///     命令常量
    /// </summary>
    public enum QQCommand : ushort
    {
        /// <summary>
        ///     保持在线状态
        /// </summary>
        Message0X0002 = 0x0002,

        /// <summary>
        ///     登录Ping
        /// </summary>
        Login0X0825 = 0x0825,

        /// <summary>
        ///     登录校验
        /// </summary>
        Login0X0836 = 0x0836,
        Login0X0839 = 0x0839,

        /// <summary>
        ///     取SessionKey
        /// </summary>
        Login0X0828 = 0x0828,

        /// <summary>
        ///     改变在线状态
        /// </summary>
        Login0X00Ec = 0x00EC,

        /// <summary>
        ///     Token请求
        /// </summary>
        Interactive0X00Ae = 0x00AE,

        /// <summary>
        ///     验证码提交
        /// </summary>
        Login0X00Ba = 0x00BA,

        /// <summary>
        ///     请求一些操作需要的密钥，比如文件中转，视频也有可能  目前用来获取Skey
        /// </summary>
        Data0X001D = 0x001D,

        /// <summary>
        ///     获取基本资料
        /// </summary>
        Data0X005C = 0x005C,

        /// <summary>
        ///     获取群分组
        /// </summary>
        Data0X0195 = 0x0195,

        /// <summary>
        ///     查询黑名单
        /// </summary>
        Data0X01A5 = 0x01A5,
        Data0X019B = 0x019B,

        /// <summary>
        ///     获取好友和群列表
        /// </summary>
        Data0X0134 = 0x0134,
        Data0X01C4 = 0x01C4,
        Data0X01C5 = 0x01C5,
        Data0X0126 = 0x0126,

        /// <summary>
        ///     天气预报
        /// </summary>
        Data0X00A6 = 0x00A6,

        /// <summary>
        ///     PM2.5浓度
        /// </summary>
        Data0X0397 = 0x0397,

        /// <summary>
        ///     问问个人中心API地址
        /// </summary>
        Data0X00D8 = 0x00D8,

        /// <summary>
        ///     群消息
        /// </summary>
        Message0X0017 = 0x0017,

        /// <summary>
        ///     群消息查看确认
        /// </summary>
        Message0X0360 = 0x0360,
        Message0X01C0 = 0x01C0,

        /// <summary>
        ///     好友消息
        /// </summary>
        Message0X00Ce = 0x00CE,

        /// <summary>
        ///     消息查看确认
        /// </summary>
        Message0X0319 = 0x0319,

        /// <summary>
        ///     发送好友消息
        /// </summary>
        Message0X00Cd = 0x00CD,

        /// <summary>
        ///     获取Ukey
        /// </summary>
        Message0X0352 = 0x0352,

        /// <summary>
        ///     获取Ukey
        /// </summary>
        Message0X0388 = 0x0388,

        /// <summary>
        ///     心跳包
        /// </summary>
        Message0X0058 = 0x0058,

        /// <summary>
        ///     点赞
        /// </summary>
        Interactive0X03E3 = 0x03E3,

        /// <summary>
        ///     未知包
        /// </summary>
        Unknown = 0xFFFF
    }

    public enum MessageType
    {
        /// <summary>
        ///     普通文本
        /// </summary>
        Normal,

        /// <summary>
        ///     @他人
        /// </summary>
        At,

        /// <summary>
        ///     系统表情
        /// </summary>
        Emoji,

        /// <summary>
        ///     图片消息
        /// </summary>
        Picture,

        /// <summary>
        ///     Xml消息
        /// </summary>
        Xml,

        /// <summary>
        ///     Json消息
        /// </summary>
        Json,

        /// <summary>
        ///     抖动
        /// </summary>
        Shake,

        /// <summary>
        ///     音频
        /// </summary>
        Audio,

        /// <summary>
        ///     视频
        /// </summary>
        Video,

        /// <summary>
        ///     发送离线文件
        /// </summary>
        OfflineFile,

        /// <summary>
        ///     退群
        /// </summary>
        ExitGroup,

        /// <summary>
        ///     获取群信息
        /// </summary>
        GetGroupImformation,

        /// <summary>
        ///     加群
        /// </summary>
        AddGroup
    }

    public static class LoginStatus
    {
        public const byte 我在线上 = 0x0A;
        public const byte Q我吧 = 0x3C;
        public const byte 离开 = 0x1E;
        public const byte 忙碌 = 0x32;
        public const byte 请勿打扰 = 0x46;
        public const byte 隐身 = 0x28;
    }

    /// <summary>
    ///     加好友类型
    /// </summary>
    public enum AddFriendType
    {
        AddFriend = 0x01,
        AddGroup = 0x02
    }

    /// <summary>
    ///     Tlv类型枚举
    /// </summary>
    public enum TlvTags
    {
        NonUinAccount = 0x0004,
        Uin = 0x0005,
        TGTGT = 0x0006,
        TGT = 0x0007,
        TimeZone = 0x0008,
        ErrorInfo = 0x000A,
        PingRedirect = 0x000C,
        _0x000D = 0x000D,
        _0x0014 = 0x0014,
        ComputerGuid = 0x0015,
        ClientInfo = 0x0017,
        Ping = 0x0018,
        GTKeyTGTGTCryptedData = 0x001A,
        GTKey_TGTGT = 0x001E,
        DeviceID = 0x001F,
        LocalIP = 0x002D,
        _0x002F = 0x002F,
        QdData = 0x0032,
        _0x0033 = 0x0033,
        LoginReason = 0x0036,
        ErrorCode = 0x0100,
        Official = 0x0102,
        SID = 0x0103,
        _0x0104 = 0x0104,
        m_vec0x12c = 0x0105,
        TicketInfo = 0x0107,
        AccountBasicInfo = 0x0108,
        _ddReply = 0x0109,
        QDLoginFlag = 0x010B,
        _0x010C = 0x010C,
        SigLastLoginInfo = 0x010D,
        _0x010E = 0x010E,
        SigPic = 0x0110,
        SigIP2 = 0x0112,
        DHParams = 0x0114,
        PacketMd5 = 0x0115,
        Ping_Strategy = 0x0309,
        ComputerName = 0x030F,
        ServerAddress = 0x0310,
        Misc_Flag = 0x0312,
        GUID_Ex = 0x0313,
        _0x0404 = 0x0404,
        _0x0508 = 0x0508,
        _0x050C = 0x050C
    }

    public enum ResultCode
    {
        成功 = 0x00,
        需要更新TGTGT = 0x01,
        帐号被回收 = 0x33,
        密码错误 = 0x34,
        需要验证密保 = 0x3F,
        DoMain = 0xF8,
        要求切换TCP = 0xF9,
        需要重新CheckTGTGT = 0xFA,
        需要验证码 = 0xFB,
        需要重定向 = 0xFE,
        过载保护 = 0xFD,
        其它错误 = 0xFF
    }
}