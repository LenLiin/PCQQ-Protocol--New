using System.Threading;
using QQ.Framework.Domains;
using QQ.Framework.Packets;
using QQ.Framework.Utils;

namespace QQ.Framework.Sockets
{
    /// <summary>
    ///     消息管理器
    /// </summary>
    public class MessageManage
    {
        /// <summary>
        ///     Socket连接服务
        /// </summary>
        private readonly SocketService _service;

        /// <summary>
        ///     账号信息
        /// </summary>
        private readonly QQUser _user;

        /// <summary>
        ///     消息转发器
        /// </summary>
        private readonly ServerMessageSubject _transponder;

        public MessageManage(SocketService service, QQUser user, ServerMessageSubject transponder)
        {
            _service = service;
            _user = user;
            _transponder = transponder;
        }

        public void Init()
        {
            _service.Login();
            ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.QueueUserWorkItem(Receive, null);
        }

        private void Receive(object state)
        {
            while (true)
            {
                var result = _service.Receive();

                var hexStr = Util.ToHex(result.Data);
                hexStr = hexStr.Substring(0, hexStr.LastIndexOf("03 00") + 2);
                //包装到ByteBuffer
                var tempBuf = Util.HexStringToByteArray(hexStr);
                //需要一个基础包 
                var _ReceivePacket = new ReceivePacket(tempBuf, _user, null);
                //接收消息后触发事件
                var ReceiveEvent = new QQEventArgs<ReceivePacket>(_service, _user, _ReceivePacket);
                _service.MessageLog($"接收数据:{Util.ToHex(ReceiveEvent.ReceivePacket.buffer)}");

                // 通过Command, 利用反射+Attribute, 分发到管理具体某个包的Command中,最后直接调用Receive方法即可。
                // 将对包的处理移到具体Command中，此处只负责分发。
                var receive_command = DispatchPacketToCommand.Of(tempBuf, _service, _transponder, _user)
                    .dispatch_receive_packet(_ReceivePacket.Command);
                receive_command.Process();
            }
        }

        #region 历史收包函数

//        private void ReciveMsg(object state)
//        {
//            while (true)
//            {
//                EndPoint RecivePoint = new IPEndPoint(IPAddress.Any, 0); //用来保存发送方的ip和端口号
//                var buffer = new byte[QQGlobal.QQ_PACKET_MAX_SIZE];
//                var length = Server.ReceiveFrom(buffer, ref RecivePoint); //接收数据报
//
//                var hexStr = Util.ToHex(buffer);
//                hexStr = hexStr.Substring(0, hexStr.LastIndexOf("03 00") + 2);
//                //包装到ByteBuffer
//                var tempBuf = Util.HexStringToByteArray(hexStr);
//                //需要一个基础包 
//                var _ReceivePacket = new ReceivePacket(tempBuf, client.QQUser, null);
//                //接收消息后触发事件
//                var ReceiveEvent = new QQEventArgs<ReceivePacket>(client, TODO, _ReceivePacket);
//                client.OnReceive(ReceiveEvent);
//                switch (_ReceivePacket.Command)
//                {
//                    case QQCommand.Login0x0825:
//                        var _0825Packet = new Receive_0x0825(tempBuf, client.QQUser);
//                        var _0825e = new QQEventArgs<Receive_0x0825>(client, TODO, _0825Packet);
//                        if (_0825Packet.DataHead == 0xFE)
//                        {
//                            client.OnReceive_0x0825Redirect(_0825e);
//                        }
//                        else
//                        {
//                            client.OnReceive_0x0825(_0825e);
//                        }
//
//                        break;
//                    case QQCommand.Login0x0836:
//                        var _0836Packet = new Receive_0x0836(tempBuf, client.QQUser);
//                        var _0836e = new QQEventArgs<Receive_0x0836>(client, TODO, _0836Packet);
//                        if (_0836Packet.GetPacketLength() == 319 || _0836Packet.GetPacketLength() == 351)
//                        {
//                            client.MessageLog("你输入的帐号名或密码不正确，原因可能是：输错帐号；记错密码；未区分字母大小写；未开启小键盘。");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 271 || _0836Packet.GetPacketLength() == 207)
//                        {
//                            client.OnReceive_0x0836_686(_0836e);
//                        }
//                        else if (_0836Packet.GetPacketLength() == 135)
//                        {
//                            client.MessageLog("抱歉，请重新输入密码");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 279)
//                        {
//                            client.MessageLog("你的帐号存在被盗风险，已进入保护模式");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 263)
//                        {
//                            client.MessageLog("你输入的帐号不存在");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 551 || _0836Packet.GetPacketLength() == 487)
//                        {
//                            client.MessageLog("你的帐号开启了设备锁，请关闭设备锁后再进行操作");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 359)
//                        {
//                            client.MessageLog("你的帐号长期未登录已被回收");
//                        }
//                        else if (_0836Packet.GetPacketLength() == 871)
//                        {
//                            client.MessageLog("需要验证码登录");
//                            client.OnReceive_0x0836_871(_0836e);
//                        }
//                        else if (_0836Packet.GetPacketLength() > 700)
//                        {
//                            client.OnReceive_0x0836_622(_0836e);
//                        }
//
//                        break;
//                    case QQCommand.Login0x0828:
//                        var _0828Packet = new Receive_0x0828(tempBuf, client.QQUser);
//                        var _0828e = new QQEventArgs<Receive_0x0828>(client, TODO, _0828Packet);
//                        client.OnReceive_0x0828(_0828e);
//
//                        //定时发送心跳包
//                        var timersInvoke = new TimersInvoke(client);
//                        timersInvoke.StartTimer();
//
//                        break;
//                    case QQCommand.Login0x00EC:
//                        var _00ECPacket = new Receive_0x00EC(tempBuf, client.QQUser);
//                        var _00ECe = new QQEventArgs<Receive_0x00EC>(client, TODO, _00ECPacket);
//                        client.OnReceive_0x00EC(_00ECe);
//                        break;
//                    case QQCommand.Data0x001D:
//                        var _001DPacket = new Receive_0x001D(tempBuf, client.QQUser);
//                        var _001De = new QQEventArgs<Receive_0x001D>(client, TODO, _001DPacket);
//                        client.OnReceive_0x001D(_001De);
//                        break;
//                    case QQCommand.Data0x005C:
//                        var _005CPacket = new Receive_0x005C(tempBuf, client.QQUser);
//                        var _005Ce = new QQEventArgs<Receive_0x005C>(client, TODO, _005CPacket);
//                        client.OnReceive_0x005C(_005Ce);
//                        break;
//                    case QQCommand.Message0x0017:
//                        var _0017Packet = new Receive_0x0017(tempBuf, client.QQUser);
//                        var _0017e = new QQEventArgs<Receive_0x0017>(client, TODO, _0017Packet);
//                        client.OnReceive_0x0017(_0017e);
//                        break;
//                    case QQCommand.Message0x00CE:
//                        var _00CEPacket = new Receive_0x00CE(tempBuf, client.QQUser);
//                        var _00CEe = new QQEventArgs<Receive_0x00CE>(client, TODO, _00CEPacket);
//                        client.OnReceive_0x00CE(_00CEe);
//                        break;
//                    case QQCommand.Message0x00CD:
//                        var _00CDPacket = new Receive_0x00CD(tempBuf, client.QQUser);
//                        var _00CDe = new QQEventArgs<Receive_0x00CD>(client, TODO, _00CDPacket);
//                        client.OnReceive_0x00CD(_00CDe);
//                        break;
//                    case QQCommand.Message0x0058:
//                        var _0058Packet = new Receive_0x0058(tempBuf, client.QQUser);
//                        var _0058e = new QQEventArgs<Receive_0x0058>(client, TODO, _0058Packet);
//                        client.OnReceive_0x0058(_0058e);
//                        break;
//                    case QQCommand.Login0x00BA:
//                        var _00BAPacket = new Receive_0x00BA(tempBuf, client.QQUser);
//                        var _00BAe = new QQEventArgs<Receive_0x00BA>(client, TODO, _00BAPacket);
//                        client.OnReceive_0x00BA(_00BAe);
//                        break;
//                    default:
//                        var _CurrencyPacket = new Receive_Currency(tempBuf, client.QQUser);
//                        var _Currencye = new QQEventArgs<Receive_Currency>(client, TODO, _CurrencyPacket);
//                        client.OnReceive_Currency(_Currencye);
//                        break;
//                }
//            }
//        }

        #endregion
    }
}