using System.Threading;
using QQ.Framework.Domains;
using QQ.Framework.Events;
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
        private readonly ISocketService _service;

        /// <summary>
        ///     账号信息
        /// </summary>
        private readonly QQUser _user;

        /// <summary>
        ///     消息转发器
        /// </summary>
        private readonly IServerMessageSubject _transponder;

        public MessageManage(ISocketService service, QQUser user, IServerMessageSubject transponder)
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
                var receivePacket = new ReceivePacket(tempBuf, _user, null);
                //接收消息后触发事件
                var receiveEvent = new QQEventArgs<ReceivePacket>(_service, _user, receivePacket);
                if (QQGlobal.DebugLog)
                {
                    _service.MessageLog($"接收数据:{Util.ToHex(receiveEvent.ReceivePacket.Buffer)}");
                }

                // 通过Command, 利用反射+Attribute, 分发到管理具体某个包的Command中,最后直接调用Receive方法即可。
                // 将对包的处理移到具体Command中，此处只负责分发。
                var receiveCommand = DispatchPacketToCommand.Of(tempBuf, _service, _transponder, _user)
                    .dispatch_receive_packet(receivePacket.Command);
                receiveCommand.Process();
            }
        }
    }
}