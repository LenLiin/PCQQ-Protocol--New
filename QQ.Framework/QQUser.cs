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
        private readonly string UA =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";

        public QQUser(long qqNum, string pwd)
        {
            QQ = qqNum;
            SetPassword(pwd);
            Initialize();
        }

        /// <summary>
        ///     经过重定向登录
        /// </summary>
        /// <value></value>
        public bool IsLoginRedirect { get; set; }

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

        public byte[] QQPacketTgtgtKey { get; set; } = Util.RandomKey();

        /// <summary>
        ///     00BA占位段(暂时未解析出具体含义)
        /// </summary>
        public byte[] QQPacket00BaFixKey { get; set; } =
        {
            0x69, 0x20, 0xD1, 0x14, 0x74, 0xF5, 0xB3,
            0x93, 0xE4, 0xD5, 0x02, 0xB3, 0x71, 0x1A, 0xCD, 0x2A
        };

        /// <summary>
        ///     0836密钥1
        /// </summary>
        public byte[] QQPacket0836Key1 { get; set; } = Util.RandomKey();

        public byte[] QQPacket00BaVerifyCode { get; set; }
        public byte QQPacket00BaSequence { get; set; } = 0x01;

        /// <summary>
        ///     加 好友/群 所需Token
        /// </summary>
        public byte[] AddFriend0018Value { get; set; }

        /// <summary>
        ///     加 好友/群 所需Token
        /// </summary>
        public byte[] AddFriend0020Value { get; set; }

        /// <summary>
        ///     密码一次MD5
        /// </summary>
        public byte[] MD51 { get; set; }

        /// <summary>
        ///     QQ号
        /// </summary>
        public long QQ { get; set; }

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
                using (var httpWebClient = new HttpWebClient())
                {
                    //string address = string.Format("http://ptlogin2.qq.com/jump?ptlang=2052&clientuin={0}&clientkey={1}&u1=http%3A%2F%2Fqzone.qq.com&ADUIN={0}&ADSESSION={2}&ADTAG=CLIENT.QQ.5365_.0&ADPUBNO=26405",
                    //    QQ, Util.ToHex(TXProtocol.ClientKey, "", "{0}"), Util.GetTimeMillis(DateTime.Now));
                    var address =
                        $"https://ssl.ptlogin2.qq.com/jump?pt_clientver=5593&pt_src=1&keyindex=9&ptlang=2052&clientuin={QQ}&clientkey={Util.ToHex(TXProtocol.BufServiceTicketHttp, "", "{0}")}&u1=https:%2F%2Fuser.qzone.qq.com%2F417085811%3FADUIN=417085811%26ADSESSION={Util.GetTimeMillis(DateTime.Now)}%26ADTAG=CLIENT.QQ.5593_MyTip.0%26ADPUBNO=26841&source=namecardhoverstar";
                    httpWebClient.Headers["User-Agent"] = UA;
                    var text = Encoding.UTF8.GetString(httpWebClient.DownloadData(address));
                    QQCookies = httpWebClient.Cookies;
                    var cookies = QQCookies.GetCookies(new Uri("http://qq.com"));
                    if (cookies["skey"] != null)
                    {
                        var value = cookies["skey"].Value;
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
                using (var httpWebClient = new HttpWebClient())
                {
                    var address = string.Format(
                        "https://ssl.ptlogin2.qq.com/jump?pt_clientver=5509&pt_src=1&keyindex=9&clientuin={0}&clientkey={1}&u1=http%3A%2F%2Fqun.qq.com%2Fmember.html%23gid%3D168209441",
                        QQ, Util.ToHex(TXProtocol.BufServiceTicketHttp /*QunKey*/, "", "{0}"),
                        Util.GetTimeMillis(DateTime.Now));
                    httpWebClient.Headers["User-Agent"] = UA;
                    var result = Encoding.UTF8.GetString(httpWebClient.DownloadData(address));
                    QunCookies = httpWebClient.Cookies;
                    var cookies = QunCookies.GetCookies(new Uri("http://qun.qq.com"));
                    if (cookies["skey"] != null && !string.IsNullOrWhiteSpace(cookies["skey"].Value))
                    {
                        QQSkey = cookies["skey"].Value;
                        Bkn = Util.GetBkn(cookies["skey"].Value);
                    }

                    var value2 = cookies["p_skey"].Value;
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
                using (var httpWebClient = new HttpWebClient())
                {
                    var address = "https://qun.qq.com/cgi-bin/qun_mgr/search_group_members";
                    var s = $"gc={ExternalId}&st=0&end=10000&sort=0&bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Cookies = QunCookies;
                    var text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST",
                        Encoding.UTF8.GetBytes(s)));

                    var r = new Regex("\"[0-9]+\":\"[^\"]+\"");
                    if (r.IsMatch(text))
                    {
                        foreach (var match in r.Matches(text))
                        {
                            var str = ((Capture) match).Value.Split(':');
                            var r2 = new Regex("\"[0-9]+\"");
                            var Level = r2.Matches(str[0])[0].Value;
                            var r3 = new Regex("\"[^\"]+\"");
                            var Name = r3.Matches(str[1])[0].Value;
                            var DataItem = "{\"level\":" + Level + ",\"name\":" + Name + "}";

                            text = text.Replace(((Capture) match).Value, DataItem);
                        }

                        text = text.Replace("\"levelname\":{", "\"levelname\":[")
                            .Replace("},\"max_count\"", "],\"max_count\"");
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
                using (var httpWebClient = new HttpWebClient())
                {
                    var address = "https://qun.qq.com/cgi-bin/qun_mgr/get_group_list";
                    var s = $"bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Cookies = QunCookies;
                    var text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST",
                        Encoding.UTF8.GetBytes(s)));

                    MessageLog("获取群列表成功:" + text);

                    var Groups = JsonConvert.DeserializeObject<GroupList>(text);
                    if (Groups.Create != null)
                    {
                        foreach (var item in Groups.Create)
                        {
                            item.Members = Search_Group_Members((long) item.Gc);
                        }
                    }

                    if (Groups.Join != null)
                    {
                        foreach (var item in Groups.Join)
                        {
                            item.Members = Search_Group_Members((long) item.Gc);
                        }
                    }

                    if (Groups.Manage != null)
                    {
                        foreach (var item in Groups.Manage)
                        {
                            item.Members = Search_Group_Members((long) item.Gc);
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

        public FriendList Get_Friend_List()
        {
            try
            {
                using (var httpWebClient = new HttpWebClient())
                {
                    var address = "https://qun.qq.com/cgi-bin/qun_mgr/get_friend_list";
                    var s = $"bkn={Bkn}";
                    httpWebClient.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                    httpWebClient.Headers["Referer"] = "http://qun.qq.com/member.html";
                    httpWebClient.Headers["X-Requested-With"] = "XMLHttpRequest";
                    httpWebClient.Headers["User-Agent"] = UA;
                    httpWebClient.Headers.Add("Cache-Control: no-cache");
                    httpWebClient.Cookies = QunCookies;
                    var text = Encoding.UTF8.GetString(httpWebClient.UploadData(address, "POST",
                        Encoding.UTF8.GetBytes(s)));
                    var r = new Regex("\"[0-9]+\":");
                    if (r.IsMatch(text))
                    {
                        foreach (var match in r.Matches(text))
                        {
                            var str = ((Capture) match).Value;
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

        #region TXSSO  TLV参数

        public TXProtocol TXProtocol { get; set; } = new TXProtocol();

        /// <summary>
        ///     好友列表
        /// </summary>
        public FriendList Friends { get; set; }

        //群列表
        public GroupList Groups { get; set; }

        #endregion
    }
}