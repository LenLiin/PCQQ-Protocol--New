using System;
using System.Collections.Generic;
using System.Net;
using QQ.Framework.Utils;

namespace QQ.Framework
{
    public class TXProtocol
    {
        public byte[] BufTgtgtKey { get; set; } = Util.RandomKey();
        public byte[] BufTgtgt { get; set; }

        public byte[] BufTgt { get; set; }

        //public byte[] bufComputerIDEx { get; set; } = Util.RandomKey();
        public byte[] BufComputerId { get; set; } =
            {0x43, 0x04, 0x21, 0x7D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        //public byte[] bufComputerID { get; set; } = Util.RandomKey();
        public byte[] BufComputerIdEx { get; set; } =
            {0x77, 0x98, 0x00, 0x0B, 0xAB, 0x5D, 0x4F, 0x3D, 0x30, 0x50, 0x65, 0x2C, 0x4A, 0x2A, 0xF8, 0x65};

        public byte[] BufDeviceId { get; set; } =
        {
            0x0f, 0xab, 0xbe, 0x21, 0x04, 0xa7, 0x2a, 0xf1, 0xe1, 0x9d, 0xa1, 0x95, 0x6a, 0x36, 0x3d, 0xf0, 0x7b, 0x22,
            0xff, 0x2e, 0xc2, 0xca, 0xc9, 0x2b, 0xa8, 0xd6, 0xda, 0x45, 0x9d, 0x31, 0xa9, 0x60
        };

        public byte[] BufSigPic { get; set; }
        public byte[] PngToken { get; set; }
        public byte[] PngKey { get; set; }
        public byte[] BufTgtGtKey { get; set; }
        public byte[] Buf16BytesGtKeySt { get; set; }
        public byte[] BufServiceTicket { get; set; }
        public byte[] Buf16BytesGtKeyStHttp { get; set; }
        public byte[] BufServiceTicketHttp { get; set; }
        public byte[] BufGtKeyTgtPwd { get; set; }
        public byte[] BufSessionKey { get; set; }
        public byte[] BufSigSession { get; set; }
        public byte[] BufPwdForConn { get; set; }
        public byte[] SessionKey { get; set; }
        public byte[] ClientKey { get; set; }
        public byte[] BufSigClientAddr { get; set; }

        public byte[] BufDhPublicKey { get; set; } =
        {
            0x02, 0x78, 0x28, 0x16, 0x7C, 0x9E, 0xF3, 0xB7, 0x5A, 0x7B, 0x5A, 0xEF, 0xA2, 0x30, 0x10, 0xEC, 0x0C, 0x46,
            0x87, 0x70, 0x76, 0x31, 0xA7, 0x88, 0xEA
        };

        public byte[] BufDhShareKey { get; set; } =
        {
            0x60, 0x42, 0x3B, 0x51, 0xC3, 0xB1, 0xF6, 0x0F, 0x67, 0xE8, 0x9C, 0x00, 0xF0, 0xA7, 0xBD, 0xA3
        };

        public byte[] BufMachineInfoGuid { get; set; }

        //public byte[] bufMacGuid { get; set; } = Util.RandomKey();
        public byte[] BufMacGuid { get; set; } =
            {0x21, 0x4B, 0x1A, 0x04, 0x09, 0xED, 0x19, 0x70, 0x98, 0x75, 0x51, 0xBB, 0x2D, 0x3A, 0x7E, 0x0A};

        #region 用户相关

        /// <summary>
        ///     记住密码
        /// </summary>
        public byte BRememberPwdLogin { get; set; } = 0x00;

        public byte CPingType { get; set; } = 0x01;

        /// <summary>
        ///     重定向IP记录
        /// </summary>
        public List<byte[]> RedirectIP { get; set; } = new List<byte[]>();

        /// <summary>
        ///     计算机名
        /// </summary>
        public string BufComputerName { get; set; } = Dns.GetHostName();

        /// <summary>
        ///     SSO主版本号
        /// </summary>
        public byte CMainVer = 0x37;

        /// <summary>
        ///     SSO次版本号
        /// </summary>
        public byte CSubVer = 0x09;

        /// <summary>
        ///     Array
        /// </summary>
        public byte[] XxooA = {0x03, 0x00, 0x00};

        public byte[] XxooD = {0x30, 0x00, 0x00, 0x00};
        public byte XxooB = 0x01;

        /// <summary>
        ///     客户端类型
        /// </summary>
        public byte[] DwClientType = {0x00, 0x01, 0x01, 0x01};

        /// <summary>
        ///     发行版本号
        /// </summary>
        public byte[] DwPubNo = {0x00, 0x00, 0x68, 0x1C};

        #region QdData 相关参数

        public ushort CQdProtocolVer = 0x0063;
        public long DwQdVerion = 0x02040404;
        public ushort WQdCsCmdNo = 0x0004;
        public byte CQdCcSubNo = 0x00;

        /// <summary>
        ///     系统类型
        /// </summary>
        internal byte COsType = 0x03;

        /// <summary>
        ///     是否x64
        /// </summary>
        internal byte BIsWow64 = 0x01;

        public long DwDrvVersionInfo = 0x01020000;

        /// <summary>
        ///     TSSafeEdit.dat的"文件版本"
        /// </summary>
        public byte[] BufVersionTsSafeEditDat = {0x07, 0xdf, 0x00, 0x0a, 0x00, 0x0c, 0x00, 0x01};

        /// <summary>
        ///     QScanEngine.dll的"文件版本"
        /// </summary>
        public byte[] BufVersionQScanEngineDll = {0x00, 0x04, 0x00, 0x03, 0x00, 0x04, 0x20, 0x5c};

        public byte[] QdSufFix = {0x68};
        public byte[] QdPreFix = {0x3E};

        /// <summary>
        ///     wE7^3img#i)%h12]
        /// </summary>
        public byte[] BufQdKey =
            {0x77, 0x45, 0x37, 0x5e, 0x33, 0x69, 0x6d, 0x67, 0x23, 0x69, 0x29, 0x25, 0x68, 0x31, 0x32, 0x5d};

        #endregion

        #endregion

        #region 全局

        /// <summary>
        ///     主版本号
        /// </summary>
        public uint DwSsoVersion { get; set; } = 0x00000453;

        public uint DwServiceId { get; set; } = 0x00000001;

        /// <summary>
        ///     客户端版本
        /// </summary>
        public uint DwClientVer { get; set; } = 0x00001585;

        public uint DwIsp { get; set; }
        public uint DwIdc { get; set; }
        public long TimeDifference { get; set; }

        public byte[] BufSid { get; set; } =
            {0x1E, 0xC1, 0x25, 0x71, 0xB2, 0x4C, 0xEA, 0x91, 0x9A, 0x6E, 0x8D, 0xE6, 0x95, 0x4E, 0xCE, 0x06};

        //public byte[] bufSID { get; set; } = Util.RandomKey();
        public byte[] QqexeMD5 { get; set; }

        #endregion

        #region 客户端

        public ushort WClientPort { get; set; }
        public string DwClientIP { get; set; }
        public DateTime DwServerTime { get; set; }

        /// <summary>
        ///     重定向次数
        /// </summary>
        public ushort WRedirectCount { get; set; }

        public string DwServerIP { get; set; } = "61.151.226.190";
        public string DwRedirectIP { get; set; }
        public ushort WRedirectPort { get; set; }
        public ushort WServerPort { get; set; } = 8000;
        public ushort SubVer { get; set; } = 0x0001;
        public ushort EcdhVer { get; set; } = 0x0102;
        public byte[] QdData { get; set; }

        #endregion
    }
}