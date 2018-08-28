using QQ.Framework.Packets;
using QQ.Framework.Packets.Receive.Data;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace QQ.Framework.Sockets
{
    /// <summary>
    /// 消息管理器
    /// </summary>
    public class MessageManage
    {
        private QQClient client;
        /// <summary>
        /// 消息收发器
        /// </summary>
        public Socket Server { get; set; }
        public MessageManage(QQClient client)
        {
            this.Server = client.Server;
            this.client = client;
            EndPoint point = new IPEndPoint(IPAddress.Parse(client.LoginServerHost), client.LoginPort);
            client.Point = point;
            //定时发送心跳包
            TimersInvoke timersInvoke = new TimersInvoke(client);
            timersInvoke.StartTimer();
        }
        public void Init()
        {
            ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ReciveMsg), client.State);
        }
        void ReciveMsg(object state)
        {
            while (true)
            {
                EndPoint RecivePoint = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[QQGlobal.QQ_PACKET_MAX_SIZE];
                int length = Server.ReceiveFrom(buffer, ref RecivePoint);//接收数据报

                var hexStr = Util.ToHex(buffer);
                hexStr = hexStr.Substring(0, hexStr.LastIndexOf("03 00") + 2);
                //包装到ByteBuffer
                ByteBuffer tempBuf = new ByteBuffer(Util.HexStringToByteArray(hexStr)) { Position = 0 };
                //需要一个基础包 
                var _ReceivePacket = new ReceivePacket(tempBuf, client.QQUser, null);
                //接收消息后触发事件
                QQEventArgs<ReceivePacket> ReceiveEvent = new QQEventArgs<ReceivePacket>(client, _ReceivePacket);
                client.OnReceive(ReceiveEvent);
                //数据包指针归零
                tempBuf.Position = 0;
                switch (_ReceivePacket.Command)
                {
                    case QQCommand.Login0x0825:
                        var _0825Packet = new Receive_0x0825(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x0825> _0825e = new QQEventArgs<Receive_0x0825>(client, _0825Packet);
                        if (_0825Packet.DataHead == 0xFE)
                        {
                            client.OnReceive_0x0825Redirect(_0825e);
                        }
                        else
                        {
                            client.OnReceive_0x0825(_0825e);
                        }
                        break;
                    case QQCommand.Login0x0836:
                        var _0836Packet = new Receive_0x0836(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x0836> _0836e = new QQEventArgs<Receive_0x0836>(client, _0836Packet);
                        if (_0836Packet.GetPacketLength() == 319|| _0836Packet.GetPacketLength() == 351)
                        {
                            client.MessageLog("你输入的帐号名或密码不正确，原因可能是：输错帐号；记错密码；未区分字母大小写；未开启小键盘。");
                        }
                        else if (_0836Packet.GetPacketLength() == 271 || _0836Packet.GetPacketLength() == 207)
                        {
                            client.OnReceive_0x0836_686(_0836e);
                        }
                        else if (_0836Packet.GetPacketLength() == 135)
                        {
                            client.MessageLog("抱歉，请重新输入密码");
                        }
                        else if (_0836Packet.GetPacketLength() == 279)
                        {
                            client.MessageLog("你的帐号存在被盗风险，已进入保护模式");
                        }
                        else if (_0836Packet.GetPacketLength() == 263)
                        {
                            client.MessageLog("你输入的帐号不存在");
                        }
                        else if (_0836Packet.GetPacketLength() == 551 || _0836Packet.GetPacketLength() == 487)
                        {
                            client.MessageLog("你的帐号开启了设备锁，请关闭设备锁后再进行操作");
                        }
                        else if (_0836Packet.GetPacketLength() == 359)
                        {
                            client.MessageLog("你的帐号长期未登录已被回收");
                        }
                        else if (_0836Packet.GetPacketLength() == 871)
                        {
                            client.MessageLog("需要验证码登录");
                            //client.OnReceive_0x0836_871(_0836e);
                        }
                        else if (_0836Packet.GetPacketLength() > 700)
                        {
                            client.OnReceive_0x0836_622(_0836e);
                        }
                        break;
                    case QQCommand.Login0x0828:
                        var _0828Packet = new Receive_0x0828(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x0828> _0828e = new QQEventArgs<Receive_0x0828>(client, _0828Packet);
                        client.OnReceive_0x0828(_0828e);
                        break;
                    case QQCommand.Login0x00EC:
                        var _00ECPacket = new Receive_0x00EC(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x00EC> _00ECe = new QQEventArgs<Receive_0x00EC>(client, _00ECPacket);
                        client.OnReceive_0x00EC(_00ECe);
                        break;
                    case QQCommand.Data0x001D:
                        var _001DPacket = new Receive_0x001D(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x001D> _001De = new QQEventArgs<Receive_0x001D>(client, _001DPacket);
                        client.OnReceive_0x001D(_001De);
                        break;
                    case QQCommand.Data0x005C:
                        var _005CPacket = new Receive_0x005C(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x005C> _005Ce = new QQEventArgs<Receive_0x005C>(client, _005CPacket);
                        client.OnReceive_0x005C(_005Ce);
                        break;
                    case QQCommand.Message0x0017:
                        var _0017Packet = new Receive_0x0017(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x0017> _0017e = new QQEventArgs<Receive_0x0017>(client, _0017Packet);
                        client.OnReceive_0x0017(_0017e);
                        break;
                    case QQCommand.Message0x00CE:
                        var _00CEPacket = new Receive_0x00CE(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x00CE> _00CEe = new QQEventArgs<Receive_0x00CE>(client, _00CEPacket);
                        client.OnReceive_0x00CE(_00CEe);
                        break;
                    case QQCommand.Message0x00CD:
                        var _00CDPacket = new Receive_0x00CD(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x00CD> _00CDe = new QQEventArgs<Receive_0x00CD>(client, _00CDPacket);
                        client.OnReceive_0x00CD(_00CDe);
                        break;
                    case QQCommand.Message0x0058:
                        var _0058Packet = new Receive_0x0058(tempBuf, client.QQUser);
                        QQEventArgs<Receive_0x0058> _0058e = new QQEventArgs<Receive_0x0058>(client, _0058Packet);
                        client.OnReceive_0x0058(_0058e);
                        break;
                    default:
                        var _CurrencyPacket = new Receive_Currency(tempBuf, client.QQUser);
                        QQEventArgs<Receive_Currency> _Currencye = new QQEventArgs<Receive_Currency>(client, _CurrencyPacket);
                        client.OnReceive_Currency(_Currencye);
                        break;
                }
            }
        }

    }
}
