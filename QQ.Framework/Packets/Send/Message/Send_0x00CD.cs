using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 发送好友消息
    /// </summary>
    public class Send_0x00CD : SendPacket
    {
        public Send_0x00CD(QQUser User, string Message, FriendMessageType messageType, long ToQQ)
           : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x00CD;
            _message = Message;
            _messageType = messageType;
            _toQQ = ToQQ;
        }
        /// <summary>
        /// 好友QQ
        /// </summary>
        long _toQQ;
        /// <summary>
        /// 消息类型
        /// </summary>
        public FriendMessageType _messageType { get; set; }
        private string _message { get; set; }
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIXVER);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            var _DateTime = Util.GetTimeSeconds(DateTime.Now);
            var MessageData =Util.HexStringToByteArray( Util.ConvertStringToHex(_message));
            if (_messageType == FriendMessageType.Shake)
            {
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                buf.Put(new byte[] { 0x37, 0x0F });
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(Util.RandomKey());
                buf.Put(new byte[] { 0x00, 0xAF });
                buf.Put(Util.RandomKey(2));
                buf.PutLong(_DateTime);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                buf.Put(Util.RandomKey(4));
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            }
            else
            {
                var _Md5 = Util.HexStringToByteArray(Util.ToHex(user.QQ_SessionKey));
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x0D, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01,0x01 });
                buf.Put(new byte[] { 0x36, 0x43 });
                buf.PutLong(user.QQ);
                buf.PutLong(_toQQ);
                buf.Put(_Md5);
                buf.Put(new byte[] { 0x00, 0x0B });
                buf.Put(Util.RandomKey(2));
                buf.PutLong(_DateTime);
                buf.Put(new byte[] { 0x02, 0x34, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x4D, 0x53, 0x47, 0x00, 0x00, 0x00, 0x00, 0x00 });
                buf.PutLong(_DateTime);
                buf.Put(Util.RandomKey(4));
                buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x86, 0x00 });
                buf.Put(new byte[] { 0x00, 0x06 });
                buf.Put(new byte[] { 0xE5, 0xAE, 0x8B, 0xE4, 0xBD, 0x93 });
                buf.Put(new byte[] { 0x00, 0x00 });
                buf.Put(new byte[] { 0x01 });
                buf.Put( 0x00 );
                buf.Put((byte)(MessageData.Length + 3));
                buf.Put(new byte[] { 0x01 });
                buf.Put(0x00);
                buf.Put((byte)MessageData.Length);
                buf.Put(MessageData);
            }
        }
    }
}