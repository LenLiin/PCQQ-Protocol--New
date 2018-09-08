using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using QQ.Framework.Packets;
using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Data;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Sockets;
using QQ.Framework.Utils;

namespace QQ.Framework
{
    public class QQClient
    {
        private readonly MessageManage messageManage;

        /// <summary>
        ///     线程池状态
        /// </summary>
        public object State = new object();

        public QQClient()
        {
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            messageManage = new MessageManage(this);
        }

        /// <summary>
        ///     连接服务
        /// </summary>
        public Socket Server { get; set; }

        public QQUser QQUser { get; set; }

        /// <summary>
        ///     服务器地址
        /// </summary>
        /// <value></value>
        public string LoginServerHost { get; set; } =
            Util.GetHostAddresses("sz2.tencent.com"); ////sz.tencent.com,sz{2-9}.tencent.com

        /// <summary>
        ///     登录端口
        /// </summary>
        /// <value></value>
        public int LoginPort { get; set; } = 8000;

        public EndPoint Point { get; set; }

        /// <summary>
        ///     是否已登录
        /// </summary>
        /// <value></value>
        public bool IsLogon { get; set; }


        public ushort wClientPort { get; set; }
        public string dwClientIP { get; set; }
        public DateTime dwServerTime { get; set; }
        public ushort wRedirectCount { get; set; }
        public string dwServerIP { get; set; }
        public ushort wServerPort { get; set; }

        public void Send(byte[] byteBuffer)
        {
            new Task(() =>
            {
                Server.SendTo(byteBuffer, Point);
                //发送数据时出发事件
                var SendEvent = new QQSendEventArgs(this, byteBuffer);
                OnSend(SendEvent);
            }).Start();
        }

        /// <summary>
        ///     日志记录
        /// </summary>
        /// <param name="str"></param>
        public void MessageLog(string str)
        {
            QQUser.MessageLog(str);
        }

        /// <summary>
        ///     登录
        /// </summary>
        public void Login()
        {
            //发送请求包
            Send(new Send_0x0825(QQUser, false).WriteData());
            MessageLog($"登陆服务器{LoginServerHost}");
            messageManage.Init();
        }

        /// <summary>
        ///     刷新IP
        /// </summary>
        public void RefPoint()
        {
            Point = new IPEndPoint(IPAddress.Parse(LoginServerHost), LoginPort);
        }

        #region 登录事件

        /// <summary>
        ///     登录事件
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0825>> EventReceive_0x0825;

        /// <summary>
        ///     登录事件
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0825(QQEventArgs<Receive_0x0825> e)
        {
            MessageLog($"连接服务器{Util.GetIpStringFromBytes(e.QQClient.QQUser.ServerIp)}成功");
            //Ping请求成功后发送0836登录包
            Send(new Send_0x0836(e.ReceivePacket.user, Login0x0836Type.Login0x0836_622).WriteData());

            EventReceive_0x0825?.Invoke(this, e);
        }

        #endregion

        #region 登录重定向事件

        /// <summary>
        ///     登录重定向事件
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0825>> EventReceive_0x0825Redirect;

        /// <summary>
        ///     登录重定向事件
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0825Redirect(QQEventArgs<Receive_0x0825> e)
        {
            MessageLog($"服务器{Util.GetIpStringFromBytes(e.QQClient.QQUser.ServerIp)}重定向");
            //如果是登陆重定向，继续登陆
            QQUser.IsLoginRedirect = true;
            LoginServerHost = Util.GetIpStringFromBytes(QQUser.ServerIp);
            //刷新重定向后服务器IP
            RefPoint();
            //重新发送登录Ping包
            Send(new Send_0x0825(e.ReceivePacket.user, true).WriteData());
            //执行事件
            EventReceive_0x0825Redirect?.Invoke(this, e);
        }

        #endregion

        #region 0836_622

        /// <summary>
        ///     0836_622
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0836>> EventReceive_0x0836_622;

        /// <summary>
        ///     0836_622
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0836_622(QQEventArgs<Receive_0x0836> e)
        {
            MessageLog("登陆成功获取个人基本信息");
            MessageLog(
                $"账号：{e.QQClient.QQUser.QQ}，昵称：{e.QQClient.QQUser.NickName}，年龄：{e.QQClient.QQUser.Age}，性别：{e.QQClient.QQUser.Gender}");
            Send(new Send_0x0828(e.ReceivePacket.user).WriteData());

            EventReceive_0x0836_622?.Invoke(this, e);
        }

        #endregion

        #region 0836_686

        /// <summary>
        ///     0836_686
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0836>> EventReceive_0x0836_686;

        /// <summary>
        ///     0836_686
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0836_686(QQEventArgs<Receive_0x0836> e)
        {
            MessageLog("二次登陆");
            //二次发送0836登录包
            Send(new Send_0x0836(e.ReceivePacket.user, Login0x0836Type.Login0x0836_686).WriteData());

            EventReceive_0x0836_686?.Invoke(this, e);
        }

        #endregion

        #region 0836_711

        /// <summary>
        ///     0836_711
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0836>> EventReceive_0x0836_711;

        /// <summary>
        ///     0836_622
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0836_711(QQEventArgs<Receive_0x0836> e)
        {
            EventReceive_0x0836_711?.Invoke(this, e);
        }

        #endregion

        #region 0836_871

        /// <summary>
        ///     0836_871
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0836>> EventReceive_0x0836_871;

        /// <summary>
        ///     0836_871
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0836_871(QQEventArgs<Receive_0x0836> e)
        {
            //请求验证码
            if (e.ReceivePacket.VerifyCommand == 0x01)
            {
                Send(new Send_0x00BA(e.ReceivePacket.user, "").WriteData());
            }

            EventReceive_0x0836_871?.Invoke(this, e);
        }

        #endregion

        #region 获取SessionKey

        /// <summary>
        ///     获取SessionKey
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0828>> EventReceive_0x0828;

        /// <summary>
        ///     0836_622
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0828(QQEventArgs<Receive_0x0828> e)
        {
            MessageLog("获取SessionKey");
            //二次发送0836登录包
            Send(new Send_0x00EC(e.ReceivePacket.user, LoginStatus.我在线上).WriteData());

            EventReceive_0x0828?.Invoke(this, e);
        }

        #endregion

        #region 00EC

        /// <summary>
        ///     00EC
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x00EC>> EventReceive_0x00EC;

        /// <summary>
        ///     00EC
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x00EC(QQEventArgs<Receive_0x00EC> e)
        {
            Send(new Send_0x001D(e.ReceivePacket.user).WriteData());

            EventReceive_0x00EC?.Invoke(this, e);
        }

        #endregion

        #region 00BA

        /// <summary>
        ///     验证码事件
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x00BA>> EventReceive_0x00BA;

        /// <summary>
        ///     验证码事件
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x00BA(QQEventArgs<Receive_0x00BA> e)
        {
            if (e.ReceivePacket.Status == 0x01)
            {
                if (e.ReceivePacket.VerifyCommand == 0x01)
                {
                    Send(new Send_0x00BA(e.ReceivePacket.user, "").WriteData());
                }
            }
            else
            {
                e.QQClient.QQUser.QQ_0836Token = e.QQClient.QQUser.QQ_PACKET_00BAVerifyToken;
                e.QQClient.QQUser.QQ_PACKET_00BASequence = 0x00;
                e.QQClient.QQUser.QQ_PACKET_TgtgtKey = Util.RandomKey();
                //验证码验证成功后发送0836登录包
                Send(new Send_0x0836(e.ReceivePacket.user, Login0x0836Type.Login0x0836_686, true).WriteData());
            }

            EventReceive_0x00BA?.Invoke(this, e);
        }

        #endregion

        #region 001D

        /// <summary>
        ///     001D
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x001D>> EventReceive_0x001D;

        /// <summary>
        ///     001D
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x001D(QQEventArgs<Receive_0x001D> e)
        {
            MessageLog("获取SKey");
            Send(new Send_0x005C(e.ReceivePacket.user).WriteData());

            EventReceive_0x001D?.Invoke(this, e);
        }

        #endregion

        #region 005C

        /// <summary>
        ///     005C
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x005C>> EventReceive_0x005C;

        /// <summary>
        ///     005C
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x005C(QQEventArgs<Receive_0x005C> e)
        {
            EventReceive_0x005C?.Invoke(this, e);
        }

        #endregion

        #region 收到群消息

        /// <summary>
        ///     收到群消息
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0017>> EventReceive_0x0017;

        /// <summary>
        ///     收到群消息
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x0017(QQEventArgs<Receive_0x0017> e)
        {
            if (!string.IsNullOrEmpty(e.ReceivePacket.Message))
            {
                if (!QQGlobal.DebugLog && e.ReceivePacket.Message.Count(c => c == '\0') > 5)
                {
                    QQUser.MessageLog($"收到群{e.ReceivePacket.Group}的{e.ReceivePacket.FromQQ}的乱码消息。");
                    //return;
                }

                QQUser.MessageLog($"收到群{e.ReceivePacket.Group}的{e.ReceivePacket.FromQQ}的消息:{e.ReceivePacket.Message}");
            }
            else
            {
                QQUser.MessageLog($"收到群{e.ReceivePacket.Group}的{e.ReceivePacket.FromQQ}的空消息。");
                //return;
            }

            //提取数据
            var dataReader = new BinaryReader(new MemoryStream(e.ReceivePacket.bodyDecrypted));

            Send(new Send_0x0017(e.ReceivePacket.user, dataReader.ReadBytes(0x10), e.ReceivePacket.Sequence)
                .WriteData());

            //if (!string.IsNullOrEmpty(e.ReceivePacket.Message))
            //{
            //    SendLongGroupMessage("<?xml version='1.0' encoding='utf-8'?><msg templateID='12345' action='web' brief='芒果科技 的分享' serviceID='2' url='http://music.163.com/song/33668486/'>  <item layout='2'>    <audio src='http://m2.music.126.net/66NgS6mnDITOLBtojRlG2g==/3359008023015680.mp3' cover='http://www.qqmango.com/xz/baoshixit.png'/><title><![CDATA[[机器人昵称]为您报时]]></title><summary><![CDATA[[时间]]]></summary>  </item>  <item layout='0'><summary><![CDATA[[星期]－[农历]]]></summary></item>  <source action='web' name='报时系统' icon='http://www.qqmango.com/xz/baoshixit.png' url='http://www.baidu.com'/></msg>",
            //      e.ReceivePacket.Group,
            //      FriendMessageType.Xml);
            //}
            //Send(new Send_0x0352(QQUser, @"D:\User\Desktop\qietu\abu_logo.png", 328601319).WriteData());
            //重复接收包不再重复触发事件并且不处理自己的消息
            if (!QQUser.ReceiveSequences.Contains(e.ReceivePacket.Sequence) &&
                !e.ReceivePacket.FromQQ.Equals(QQUser.QQ))
            {
                //回复已接收成功
                //dataReader = new BinaryReader(new MemoryStream(e.ReceivePacket.bodyDecrypted));
                //Send(new Send_0x0360(e.ReceivePacket.user, e.ReceivePacket.Group, e.ReceivePacket.ReceiveTime)
                //    .WriteData());

                QQUser.ReceiveSequences.Add(e.ReceivePacket.Sequence);
                EventReceive_0x0017?.Invoke(this, e);
            }
        }

        #endregion

        #region 收到好友消息

        /// <summary>
        ///     收到好友消息
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x00CE>> EventReceive_0x00CE;

        /// <summary>
        ///     收到好友消息
        /// </summary>
        /// <param name="e"></param>
        internal void OnReceive_0x00CE(QQEventArgs<Receive_0x00CE> e)
        {
            if (!string.IsNullOrEmpty(e.ReceivePacket.Message))
            {
                if (!QQGlobal.DebugLog && e.ReceivePacket.Message.Count(c => c == '\0') > 5)
                {
                    QQUser.MessageLog($"收到好友{e.ReceivePacket.FromQQ}的乱码消息。");
                    //return;
                }

                QQUser.MessageLog($"收到好友{e.ReceivePacket.FromQQ}的消息:{e.ReceivePacket.Message}");
            }
            else
            {
                QQUser.MessageLog($"收到好友{e.ReceivePacket.FromQQ}的空消息。");
                //return;
            }

            //提取数据
            var dataReader = new BinaryReader(new MemoryStream(e.ReceivePacket.bodyDecrypted));

            Send(new Send_0x00CE(e.ReceivePacket.user, dataReader.ReadBytes(0x10), e.ReceivePacket.Sequence)
                .WriteData());

            //SendLongUserMessage("嘿嘿嘿[face1.gif]嘿嘿嘿[face2.gif]哈哈哈嘿嘿嘿[face1.gif]嘿嘿嘿[face2.gif]哈哈哈嘿嘿嘿[face1.gif]嘿嘿嘿[face2.gif]哈哈哈嘿嘿嘿[face1.gif]嘿嘿嘿[face2.gif]哈哈哈嘿嘿嘿[face1.gif]嘿嘿嘿[face2.gif]哈哈哈", e.ReceivePacket.FromQQ);

            //重复接收包不再重复触发事件并且不处理自己的消息
            if (!QQUser.ReceiveSequences.Contains(e.ReceivePacket.Sequence) &&
                !e.ReceivePacket.FromQQ.Equals(QQUser.QQ))
            {
                //回复已接收成功
                Send(new Send_0x0319(e.ReceivePacket.user, e.ReceivePacket.FromQQ, e.ReceivePacket.MessageDateTime)
                    .WriteData());

                QQUser.ReceiveSequences.Add(e.ReceivePacket.Sequence);
                EventReceive_0x00CE?.Invoke(this, e);
            }
        }

        #endregion

        #region 发送好友消息的回复包

        /// <summary>
        ///     发送好友消息的回复包
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x00CD>> EventReceive_0x00CD;

        /// <summary>
        ///     发送好友消息的回复包
        /// </summary>
        internal void OnReceive_0x00CD(QQEventArgs<Receive_0x00CD> e)
        {
            EventReceive_0x00CD?.Invoke(this, e);
        }

        #endregion

        #region 发送群消息的回复包

        /// <summary>
        ///     发送群消息的回复包
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0002>> EventReceive_0x0002;

        /// <summary>
        ///     发送群消息的回复包
        /// </summary>
        internal void OnReceive_0x0002(QQEventArgs<Receive_0x0002> e)
        {
            EventReceive_0x0002?.Invoke(this, e);
        }

        #endregion

        #region 收到消息时

        /// <summary>
        ///     收到消息时
        /// </summary>
        public event EventHandler<QQEventArgs<ReceivePacket>> EventReceive;

        /// <summary>
        ///     收到消息时
        /// </summary>
        public void OnReceive(QQEventArgs<ReceivePacket> e)
        {
            if (QQGlobal.DebugLog)
            {
                MessageLog($"接收数据:{Util.ToHex(e.ReceivePacket.buffer)}");
            }

            EventReceive?.Invoke(this, e);
        }

        #endregion

        #region 发送消息时

        /// <summary>
        ///     发送消息时
        /// </summary>
        public event EventHandler<QQSendEventArgs> EventSend;

        /// <summary>
        ///     发送消息时
        /// </summary>
        /// <param name="e"></param>
        public void OnSend(QQSendEventArgs e)
        {
            if (QQGlobal.DebugLog)
            {
                MessageLog($"发送数据:{Util.ToHex(e.byteBuffer.ToArray())}");
            }

            EventSend?.Invoke(this, e);
        }

        #endregion

        #region 通用未知响应消息

        /// <summary>
        ///     通用未知响应消息
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_Currency>> EventReceive_Currency;

        /// <summary>
        ///     通用未知响应消息
        /// </summary>
        internal void OnReceive_Currency(QQEventArgs<Receive_Currency> e)
        {
            //提取数据
            //ByteBuffer DataBuf = new ByteBuffer(e.ReceivePacket.bodyDecrypted);
            //var buf = new ByteBuffer();
            //new Send_Currency(e.ReceivePacket.user, DataBuf.ReadBytes(0x10), e.ReceivePacket.Sequence,(char)e.ReceivePacket.GetQQCommand()).Fill(buf);
            //Send(buf);
            EventReceive_Currency?.Invoke(this, e);
        }

        #endregion

        #region 心跳包

        /// <summary>
        ///     心跳包
        /// </summary>
        public event EventHandler<QQEventArgs<Receive_0x0058>> EventReceive_0x0058;

        /// <summary>
        ///     心跳包
        /// </summary>
        internal void OnReceive_0x0058(QQEventArgs<Receive_0x0058> e)
        {
            EventReceive_0x0058?.Invoke(this, e);
        }

        #endregion

        #region 方法集

        /// <summary>
        ///     发送群消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="group"></param>
        /// <param name="MessageType">消息类型</param>
        public void SendLongGroupMessage(string message, long group, MessageType MessageType)
        {
            message = message.Replace("\n", "\r").Trim();
            foreach (var packet in Send_0x0002.SendLongMessage(QQUser, message, MessageType, group))
            {
                Send(packet.WriteData());
            }
        }

        /// <summary>
        ///     发送好友消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="MessageType">消息类型</param>
        public void SendLongUserMessage(string message, long user, MessageType MessageType)
        {
            message = message.Replace("\n", "\r").Trim();
            foreach (var packet in Send_0x00CD.SendLongMessage(QQUser, message, MessageType, user))
            {
                Send(packet.WriteData());
            }
        }

        #endregion
    }
}