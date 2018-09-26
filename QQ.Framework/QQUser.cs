using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using QQ.Framework.HttpEntity;
using QQ.Framework.Utils;

namespace QQ.Framework
{
    public class QQUser
    {
        public QQUser(long qqNum, string pwd)
        {
            QQ = qqNum;
            SetPassword(pwd);
            Initialize();
        }

        ///// <summary>
        ///// QQTIM1.0 PUBLICKEY
        ///// </summary>
        public byte[] QQPublicKey { get; set; } =
        {
            0x02, 0x6D, 0x28, 0x41, 0xD2, 0xA5, 0x6F, 0xD2, 0xFC,
            0x3E, 0x2A, 0x1F, 0x03, 0x75, 0xDE, 0x6E, 0x28, 0x8F, 0xA8, 0x19, 0x3E, 0x5F, 0x16, 0x49, 0xD3
        };

        ///// <summary>
        ///// QQ8.8 PUBLICKEY
        ///// </summary>
        //public byte[] QQ_PUBLIC_KEY{ get; set; } =
        //{
        //    0x02,0x78,0x28,0x16,0x7C,0x9E,0xF3,0xB7,0x5A,0x7B,0x5A,0xEF,0xA2,0x30,0x10,0xEC,0x0C,0x46,0x87,0x70,0x76,0x31,0xA7,0x88,0xEA
        //};

        /// <summary>
        ///     QQTIM1.0 SHAREKEY
        /// </summary>
        public byte[] QQShareKey { get; set; } =
        {
            0x1A, 0xE9, 0x7F, 0x7D, 0xC9, 0x73, 0x75, 0x98, 0xAC,
            0x02, 0xE0, 0x80, 0x5F, 0xA9, 0xC6, 0xAF
        };

        ///// <summary>
        ///// QQ8.8 SHAREKEY
        ///// </summary>
        //public byte[] QQ_SHARE_KEY { get; set; } =
        //{
        //    0x60,0x42,0x3B,0x51,0xC3,0xB1,0xF6,0x0F,0x67,0xE8,0x9C,0x00,0xF0,0xA7,0xBD,0xA3
        //};

        /// <summary>
        ///     经过重定向登录
        /// </summary>
        /// <value></value>
        public bool IsLoginRedirect { get; set; }

        /// <summary>
        ///     包体占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacketFixver { get; set; } =
            {0x03, 0x00, 0x00, 0x00, 0x01, 0x2E, 0x01, 0x00, 0x00, 0x68, 0x52, 0x00, 0x00, 0x00, 0x00};

        /// <summary>
        ///     登录包包体占位段0(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacket0825Data0 { get; set; } = {0x00, 0x18, 0x00, 0x16, 0x00, 0x01};

        /// <summary>
        ///     登录包包体占位段2(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacket0825Data2 { get; set; } =
            {0x00, 0x00, 0x04, 0x53, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x15, 0x85};

        /// <summary>
        ///     登录包密钥
        /// </summary>
        public byte[] QQPacket0825Key { get; set; } = Util.RandomKey();

        /// <summary>
        ///     重定向密钥
        /// </summary>
        public byte[] QQPacketRedirectionkey { get; set; } = Util.RandomKey();

        /// <summary>
        ///     验证码报文秘钥
        /// </summary>
        public byte[] QQPacket00BaKey { get; set; } = Util.RandomKey();


        /// <summary>
        ///     0836登录包包体占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacket0836Fix { get; set; } =
        {
            0x06, 0xA9, 0x12, 0x97, 0xB7, 0xF8, 0x76,
            0x25, 0xAF, 0xAF, 0xD3, 0xEA, 0xB4, 0xC8, 0xBC, 0xE7
        };

        public byte[] QQPacketTgtgtKey { get; set; } = Util.RandomKey();
        public byte[] QQPacketCrc32Code { get; set; } = Util.RandomKey();

        /// <summary>
        ///     00BA占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacket00BaFixKey { get; set; } =
        {
            0x69, 0x20, 0xD1, 0x14, 0x74, 0xF5, 0xB3,
            0x93, 0xE4, 0xD5, 0x02, 0xB3, 0x71, 0x1A, 0xCD, 0x2A
        };

        /// <summary>
        ///     占位段1(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacketFix1 { get; set; } =
        {
            0x03, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00,
            0x00, 0x68, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x03, 0x00, 0x19
        };

        /// <summary>
        ///     占位段2(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacketFix2 { get; set; } =
        {
            0x00, 0x15, 0x00, 0x30, 0x00, 0x01, 0x01, 0x27,
            0x9B, 0xC7, 0xF5, 0x00, 0x10, 0x65, 0x03, 0xFD,
            0x8B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x02, 0x90, 0x49, 0x55, 0x33,
            0x00, 0x10, 0x15, 0x74, 0xC4, 0x89, 0x85, 0x7A, 0x19, 0xF5,
            0x5E, 0xA9, 0xC9, 0xA3, 0x5E, 0x8A, 0x5A, 0x9B
        };

        /// <summary>
        ///     0836密钥1
        /// </summary>
        public byte[] QQPacket0836Key1 { get; set; } = Util.RandomKey();

        public byte[] QQDeviceId { get; set; } =
        {
            0x1A, 0x68, 0x73, 0x66, 0xE4, 0xBA, 0x79, 0x92, 0xCC, 0xC2, 0xD4, 0xEC, 0x14, 0x7C, 0x8B, 0xAF, 0x43, 0xB0,
            0x62, 0xFB, 0x65, 0x58, 0xA9, 0xEB, 0x37, 0x55, 0x1D, 0x26, 0x13, 0xA8, 0xE5, 0x3D
        };

        /// <summary>
        ///     客户端Key
        /// </summary>
        public byte[] QQClientKey { get; set; }

        public byte[] Qqtlv0006Encr { get; set; }
        public byte[] Qqtlv001AEncr { get; set; }
        public byte[] Qqtlv0105 { get; set; }
        public byte[] QQPacket00BaToken { get; set; }
        public byte[] QQPacket00BaVerifyToken { get; set; }
        public byte[] QQPacket00BaVerifyCode { get; set; }
        public byte QQPacket00BaSequence { get; set; } = 0x01;

        /// <summary>
        ///     0828解密密钥
        /// </summary>
        public byte[] QQ0828RecDecrKey { get; set; }

        /// <summary>
        ///     0828加密密钥
        /// </summary>
        public byte[] QQ0828RecEcrKey { get; set; }

        /// <summary>
        ///     0825Token
        /// </summary>
        public byte[] QQ0825Token { get; set; }

        public byte[] QQ0836Token { get; set; }
        public byte[] QQ0836038Token { get; set; }
        public byte[] QQ0836088Token { get; set; }

        /// <summary>
        ///     加 好友/群 所需Token
        /// </summary>
        public byte[] AddFriend0018Value { get; set; }

        /// <summary>
        ///     加 好友/群 所需Token
        /// </summary>
        public byte[] AddFriend0020Value { get; set; }

        /// <summary>
        ///     MD5_32
        /// </summary>
        public byte[] MD532 { get; set; } = Util.RandomKey(32);

        /// <summary>
        ///     登录令牌
        /// </summary>
        public byte[] QQSessionKey { get; set; }

        /// <summary>
        ///     MD5处理的用户密码
        /// </summary>
        public byte[] PasswordKey { get; private set; }

        /// <summary>
        ///     密码一次MD5
        /// </summary>
        public byte[] MD51 { get; set; }

        /// <summary>
        ///     QQ号
        /// </summary>
        public long QQ { get; set; }

        /// <summary>
        ///     本地端口，在QQ中其实只有两字节
        /// </summary>
        public int Port { get; set; }


        /// <summary>
        ///     上一次登陆时间，在QQ中其实只有4字节
        /// </summary>
        public byte[] LastLoginTime { get; set; }

        /// <summary>
        ///     本次登陆服务器时间
        /// </summary>
        public byte[] LoginTime { get; set; }

        /// <summary>
        ///     当前登陆状态，为true表示已经登陆
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        ///     登陆模式，隐身还是非隐身
        /// </summary>
        public LoginMode LoginMode { get; set; }

        /// <summary>
        ///     设置登陆服务器的方式是UDP还是TCP 默认UDP
        /// </summary>
        public bool IsUdp { get; set; } = true;

        /// <summary>
        ///     计算机名
        /// </summary>
        public string PcName { get; set; } = Dns.GetHostName();

        /// <summary>
        ///     昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///     年龄
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public byte Gender { get; set; }

        public string QQSkey { get; set; }
        public string QQPSkey { get; set; }
        public string QQGtk { get; set; }
        public string Bkn { get; set; }

        public string QunPSkey { get; set; }
        public string QunGtk { get; set; }

        public CookieContainer QQCookies { get; set; }
        public CookieContainer QunCookies { get; set; }

        /// <summary>
        ///     已接收数据包序号集合
        /// </summary>
        public List<char> ReceiveSequences { get; set; } = new List<char>();

        public string Ukey { get; set; }

        #region TXSSO  TLV参数

        public TXProtocol TXProtocol { get; set; } = new TXProtocol();

        /// <summary>
        /// 好友列表
        /// </summary>
        public FriendList Friends { get; set; }
        //群列表
        public GroupList Groups { get; set; }
        #endregion

        private void Initialize()
        {
            IsLoggedIn = false;
            LoginMode = LoginMode.Normal;
            IsUdp = true;
        }

        /// <summary>
        ///     日志记录
        /// </summary>
        /// <param name="str"></param>
        public void MessageLog(string str)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}--{str}");
        }

        public bool GetCookies()
        {
            try
            {
                using (HttpWebClient httpWebClient = new HttpWebClient())
                {
                    //string address = string.Format("http://ptlogin2.qq.com/jump?ptlang=2052&clientuin={0}&clientkey={1}&u1=http%3A%2F%2Fqzone.qq.com&ADUIN={0}&ADSESSION={2}&ADTAG=CLIENT.QQ.5365_.0&ADPUBNO=26405",
                    //    QQ, Util.ToHex(TXProtocol.ClientKey, "", "{0}"), Util.GetTimeMillis(DateTime.Now));
                    string address = $"https://ssl.ptlogin2.qq.com/jump?pt_clientver=5593&pt_src=1&keyindex=9&ptlang=2052&clientuin={QQ}&clientkey={Util.ToHex(TXProtocol.BufServiceTicketHttp, "", "{0}")}&u1=https:%2F%2Fuser.qzone.qq.com%2F417085811%3FADUIN=417085811%26ADSESSION={Util.GetTimeMillis(DateTime.Now)}%26ADTAG=CLIENT.QQ.5593_MyTip.0%26ADPUBNO=26841&source=namecardhoverstar";
                    httpWebClient.Headers["User-Agent"] = UA;
                    string text = Encoding.UTF8.GetString(httpWebClient.DownloadData(address));
                    QQCookies = httpWebClient.Cookies;
                    CookieCollection cookies = QQCookies.GetCookies(new Uri("http://qq.com"));
                    if (cookies["skey"] != null)
                    {
                        string value = cookies["skey"].Value;
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            QQSkey = value;
                            Bkn = Util.GetBkn(value);
                            QQGtk = Util.GET_GTK(value);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageLog("获取skey失败:" + ex.Message);
            }
            return false;
        }

        public bool GetQunCookies()
        {
            try
            {
                using (HttpWebClient httpWebClient = new HttpWebClient())
                {
                    string address = string.Format("https://ssl.ptlogin2.qq.com/jump?pt_clientver=5509&pt_src=1&keyindex=9&clientuin={0}&clientkey={1}&u1=http%3A%2F%2Fqun.qq.com%2Fmember.html%23gid%3D168209441",
                        QQ, Util.ToHex(TXProtocol.BufServiceTicketHttp/*QunKey*/, "", "{0}"), Util.GetTimeMillis(DateTime.Now));
                    httpWebClient.Headers["User-Agent"] = UA;
                    var result = Encoding.UTF8.GetString(httpWebClient.DownloadData(address));
                    QunCookies = httpWebClient.Cookies;
                    CookieCollection cookies = QunCookies.GetCookies(new Uri("http://qun.qq.com"));
                    if (cookies["skey"] != null && !string.IsNullOrWhiteSpace(cookies["skey"].Value))
                    {
                        QQSkey = cookies["skey"].Value;
                        Bkn = Util.GetBkn(cookies["skey"].Value);
                    }
                    string value2 = cookies["p_skey"].Value;
                    if (!string.IsNullOrWhiteSpace(value2))
                    {
                        QunPSkey = cookies["p_skey"].Value;
                        QunGtk = Util.GET_GTK(value2);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageLog("获取skey失败:" + ex.Message);
            }
            return false;
        }
        public GroupMembers Search_Group_Members(long ExternalId)
        {
            try
            {
                using (HttpWebClient httpWebClient = new HttpWebClient())
                {
                    string address = "https://qun.qq.com/cgi-bin/qun_mgr/search_group_members";
                    string s = $"gc={ExternalId}&st=0&end=10000&sort=0&bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Cookies = QunCookies;
                    string text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST", Encoding.UTF8.GetBytes(s)));

                    Regex r = new Regex("\"[0-9]+\":\"[^\"]+\"");
                    if (r.IsMatch(text))
                    {
                        foreach (var match in r.Matches(text))
                        {
                            var str = ((Capture)match).Value.Split(':');
                            Regex r2 = new Regex("\"[0-9]+\"");
                            var Level = r2.Matches(str[0])[0].Value;
                            Regex r3 = new Regex("\"[^\"]+\"");
                            var Name = r3.Matches(str[1])[0].Value;
                            var DataItem = "{\"level\":" + Level + ",\"name\":" + Name + "}";

                            text = text.Replace(((Capture)match).Value, DataItem);
                        }
                        text = text.Replace("\"levelname\":{", "\"levelname\":[").Replace("},\"max_count\"", "],\"max_count\"");
                    }
                    MessageLog($"获取群{ExternalId}成员列表成功:{text}");
                    return JsonConvert.DeserializeObject<GroupMembers>(text);
                }
            }
            catch (Exception ex)
            {
                MessageLog($"获取群{ExternalId}成员列表失败:{ex.Message}");
            }
            return null;
        }
        public GroupList Get_Group_List()
        {
            try
            {
                using (HttpWebClient httpWebClient = new HttpWebClient())
                {
                    string address = "https://qun.qq.com/cgi-bin/qun_mgr/get_group_list";
                    string s = $"bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Cookies = QunCookies;
                    string text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST", Encoding.UTF8.GetBytes(s)));

                    MessageLog("获取群列表成功:" + text);

                    var Groups = JsonConvert.DeserializeObject<GroupList>(text);
                    if (Groups.create != null)
                    {
                        foreach (var item in Groups.create)
                        {
                            item.Members = Search_Group_Members((long)item.gc);
                        }
                    }
                    if (Groups.join != null)
                    {
                        foreach (var item in Groups.join)
                        {
                            item.Members = Search_Group_Members((long)item.gc);
                        }
                    }
                    if (Groups.manage != null)
                    {
                        foreach (var item in Groups.manage)
                        {
                            item.Members = Search_Group_Members((long)item.gc);
                        }
                    }
                    return Groups;
                }
            }
            catch (Exception ex)
            {
                MessageLog("获取群列表失败:" + ex.Message);
            }
            return null;
        }
        string UA = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
        public FriendList Get_Friend_List()
        {
            try
            {
                using (HttpWebClient httpWebClient = new HttpWebClient())
                {
                    string address = "https://qun.qq.com/cgi-bin/qun_mgr/get_friend_list";
                    string s = $"bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Cookies = QunCookies;
                    string text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST", Encoding.UTF8.GetBytes(s)));
                    Regex r = new Regex("\"[0-9]+\":");
                    if (r.IsMatch(text))
                    {
                        foreach (var match in r.Matches(text))
                        {
                            var str = ((Capture)match).Value;
                            text = text.Replace(str, "");
                        }
                        text = text.Replace("\"result\":{{", "\"result\":[{").Replace("\"}}}", "\"}]}");
                    }
                    MessageLog("获取好友列表成功:" + text);
                    return JsonConvert.DeserializeObject<FriendList>(text);
                }
            }
            catch (Exception ex)
            {
                MessageLog("获取好友列表失败:" + ex.Message);
            }
            return null;
        }

        /// <summary>
        ///     设置用户的密码，不会保存明文形式的密码，立刻用Double MD5算法加密
        /// </summary>
        /// <param name="pwd">明文形式的密码</param>
        public void SetPassword(string pwd)
        {
            MD51 = QQTea.MD5(Util.GetBytes(pwd));
            PasswordKey = QQTea.MD5(QQTea.MD5(Util.GetBytes(pwd)));
        }

        /// <summary>
        ///     密码加密码一次MD5拼接后MD5加密
        /// </summary>
        /// <returns></returns>
        public byte[] Md52()
        {
            var byteBuffer = new BinaryWriter(new MemoryStream());
            byteBuffer.Write(MD51);
            byteBuffer.BeWrite(0);
            byteBuffer.BeWrite(QQ);
            return MD5.Create().ComputeHash(((MemoryStream) byteBuffer.BaseStream).ToArray());
        }
    }
}