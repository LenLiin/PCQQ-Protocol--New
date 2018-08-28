using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework
{
    public class QQUser
    {

        public byte[] QQ_PUBLIC_KEY { get; set; } = new byte[] { 0x02, 0x6D, 0x28, 0x41, 0xD2, 0xA5, 0x6F, 0xD2, 0xFC,
            0x3E, 0x2A, 0x1F, 0x03, 0x75, 0xDE, 0x6E, 0x28, 0x8F, 0xA8, 0x19, 0x3E, 0x5F, 0x16, 0x49, 0xD3 };
        public byte[] QQ_SHARE_KEY { get; set; } = new byte[] { 0x1A, 0xE9, 0x7F, 0x7D, 0xC9, 0x73, 0x75, 0x98, 0xAC,
            0x02, 0xE0, 0x80, 0x5F, 0xA9, 0xC6, 0xAF };
        public QQUser(long qqNum, string pwd)
        {
            this.QQ = qqNum;
            SetPassword(pwd);
            Initialize();
        }
        private void Initialize()
        {
            IP = new byte[4];
            ServerIp = new byte[4];
            LastLoginIp = new byte[4];
            IsLoggedIn = false;
            LoginMode = LoginMode.Normal;
            IsUdp = true;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="str"></param>
        public void MessageLog(string str)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}--{str}");
        }
        /// <summary>
        /// 经过重定向登录
        /// </summary>
        /// <value></value>
        public bool IsLoginRedirect { get; set; }
        /// <summary>
        /// 设置用户的密码，不会保存明文形式的密码，立刻用Double MD5算法加密
        /// </summary>
        /// <param name="pwd">明文形式的密码</param>
        public void SetPassword(string pwd)
        {
            MD51 = QQTea.MD5(Util.GetBytes(pwd));
            PasswordKey = QQTea.MD5(QQTea.MD5(Util.GetBytes(pwd)));
        }
        /// <summary>
        /// 包体占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_FIXVER { get; set; } = new byte[] { 0x03, 0x00, 0x00, 0x00, 0x01, 0x2E, 0x01, 0x00, 0x00, 0x68, 0x52, 0x00, 0x00, 0x00, 0x00 };
        
        /// <summary>
        /// 登录包包体占位段0(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_0825DATA0 { get; set; } = new byte[] { 0x00, 0x18, 0x00, 0x16, 0x00, 0x01 };

        /// <summary>
        /// 登录包包体占位段2(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_0825DATA2 { get; set; } = new byte[] { 0x00, 0x00, 0x04, 0x53, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x15, 0x85 };

        /// <summary>
        /// 登录包密钥
        /// </summary>
        public byte[] QQ_PACKET_0825KEY { get; set; } = Util.RandomKey();

        /// <summary>
        /// 重定向密钥
        /// </summary>
        public byte[] QQ_PACKET_REDIRECTIONKEY { get; set; } = Util.RandomKey();


        /// <summary>
        /// 0836登录包包体占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_0836FIX { get; set; } = new byte[] { 0x06, 0xA9, 0x12, 0x97, 0xB7, 0xF8, 0x76,
            0x25, 0xAF, 0xAF, 0xD3, 0xEA, 0xB4, 0xC8, 0xBC, 0xE7 };
        public byte[] QQ_PACKET_TgtgtKey { get; set; } = Util.RandomKey();
        public byte[] QQ_PACKET_Crc32_Code { get; set; } = Util.RandomKey();
        /// <summary>
        /// 00BA密钥
        /// </summary>
        public byte[] QQ_PACKET_00BA_KEY { get; set; } = new byte[] { 0xC1, 0x9C, 0xB8, 0xC8, 0x7B, 0x8C, 0x81, 0xBA,
            0x9E, 0x9E, 0x7A, 0x89, 0xE1, 0x7A, 0xEC, 0x94 };

        /// <summary>
        /// 00BA占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_00BA_FixKey { get; set; } = new byte[] { 0x69, 0x20, 0xD1, 0x14, 0x74, 0xF5, 0xB3,
            0x93, 0xE4, 0xD5, 0x02, 0xB3, 0x71, 0x1A, 0xCD, 0x2A };

        /// <summary>
        /// 占位段1(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_FIX1 { get; set; } = new byte[] { 0x03, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
            0x00, 0x68, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x03, 0x00, 0x19 };
        /// <summary>
        /// 占位段2(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQ_PACKET_FIX2 { get; set; } = new byte[] {
            0x00, 0x15, 0x00, 0x30, 0x00, 0x01, 0x01, 0x27,
            0x9B, 0xC7, 0xF5, 0x00, 0x10, 0x65, 0x03, 0xFD,
            0x8B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x02, 0x90, 0x49, 0x55, 0x33,
            0x00, 0x10, 0x15, 0x74, 0xC4, 0x89, 0x85, 0x7A, 0x19, 0xF5,
            0x5E, 0xA9, 0xC9, 0xA3, 0x5E, 0x8A, 0x5A, 0x9B
        };
        /// <summary>
        /// 0836密钥1
        /// </summary>
        public byte[] QQ_PACKET_0836_KEY1 { get; set; } = Util.RandomKey();

        public byte[] QQ_DeviceID { get; set; } = new byte[] { 0x1A, 0x68, 0x73, 0x66, 0xE4, 0xBA, 0x79, 0x92, 0xCC, 0xC2, 0xD4, 0xEC, 0x14, 0x7C, 0x8B, 0xAF, 0x43, 0xB0, 0x62, 0xFB, 0x65, 0x58, 0xA9, 0xEB, 0x37, 0x55, 0x1D, 0x26, 0x13, 0xA8, 0xE5, 0x3D };
        /// <summary>
        /// 客户端Key
        /// </summary>
        public byte[] QQ_ClientKey { get; set; }
        public byte[] QQ_tlv_0006_encr { get; set; }
        public byte[] QQ_tlv_001A_encr { get; set; }
        public byte[] QQ_tlv_0105 { get; set; }
        /// <summary>
        /// 0828解密密钥
        /// </summary>
        public byte[] QQ_0828_rec_decr_key { get; set; }
        /// <summary>
        /// 0828加密密钥
        /// </summary>
        public byte[] QQ_0828_rec_ecr_key { get; set; }
        /// <summary>
        /// 0825Token
        /// </summary>
        public byte[] QQ_0825Token { get; set; }
        public byte[] QQ_0836Token { get; set; }
        public byte[] QQ_0836_038Token { get; set; }
        public byte[] QQ_0836_088Token { get; set; }
        /// <summary>
        /// MD5_32
        /// </summary>
        public byte[] MD5_32 { get; set; } = Util.RandomKey(32);

        /// <summary>
        /// 登录令牌
        /// </summary>
        public byte[] QQ_SessionKey { get; set; }

        /// <summary>
        /// MD5处理的用户密码
        /// </summary>
        public byte[] PasswordKey { get; private set; }
        /// <summary>
        /// 密码一次MD5
        /// </summary>
        public byte[] MD51 { get; set; }
        /// <summary>
        /// 密码加密码一次MD5拼接后MD5加密
        /// </summary>
        /// <returns></returns>
        public byte[] Md52()
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.Put(Util.HexStringToByteArray(Util.ToHex(MD51)));
            byteBuffer.PutInt(0);
            byteBuffer.PutLong(QQ);
            byte[] data = byteBuffer.ToByteArray();
            byte[] arg = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return arg;
        }
        /// <summary>
        /// 本地IP
        /// </summary>
        public byte[] IP { get; set; }

        /// <summary>
        /// 上一次登陆IP
        /// </summary>
        public byte[] LastLoginIp { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public long QQ { get; set; }

        /// <summary>
        /// 本地端口，在QQ中其实只有两字节
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public byte[] ServerIp { get; set; }

        /// <summary>
        /// 服务器端口，在QQ中其实只有两字节
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 上一次登陆时间，在QQ中其实只有4字节
        /// </summary>
        public byte[] LastLoginTime { get; set; }

        /// <summary>
        /// 本次登陆时间
        /// </summary>
        public byte[] LoginTime { get; set; }

        /// <summary>
        /// 当前登陆状态，为true表示已经登陆
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// 登陆模式，隐身还是非隐身
        /// </summary>
        public LoginMode LoginMode { get; set; }

        /// <summary>
        /// 设置登陆服务器的方式是UDP还是TCP 默认UDP
        /// </summary>
        public bool IsUdp { get; set; } = true;
        /// <summary>
        /// 计算机名
        /// </summary>
        public string PcName { get; set; } = Dns.GetHostName();
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public byte Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public byte Gender { get; set; }
        public string QQ_Skey { get; set; }
        public string QQ_Cookies { get; set; }
        public string QQ_Gtk { get; set; }

        /// <summary>
        /// 已接收数据包序号集合
        /// </summary>
        public List<char> ReceiveSequences { get; set; } = new List<char>();
    }
}
