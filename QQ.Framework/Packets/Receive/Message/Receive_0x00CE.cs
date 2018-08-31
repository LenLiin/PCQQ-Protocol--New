using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    /// 好友消息
    /// </summary>
    public class Receive_0x00CE : ReceivePacket
    {
        /// <summary>
        /// 消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public byte[] MessageType { get; set; }

        /// <summary>
        /// 消息长度
        /// </summary>
        public char MessageLength { get; set; }

        /// <summary>
        /// 消息时间
        /// </summary>
        public byte[] MessageDateTime { get; set; }

        /// <summary>
        /// 消息字体
        /// </summary>
        public byte[] FontStyle { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageData { get; set; }

        /// <summary>
        /// 好友消息
        /// </summary>
        public Receive_0x00CE(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            FromQQ = (long) Util.GetQQNumRetUint(Util.ToHex(reader.ReadBytes(4)));
            reader.ReadBytes(4); //自己的QQ
            reader.ReadBytes(10);
            MessageType = reader.ReadBytes(2);
            reader.BEReadChar();
            reader.ReadBytes(reader.BEReadChar()); //未知
            reader.BEReadChar(); //消息来源QQ的版本号
            reader.ReadBytes(4); //FromQQ
            reader.ReadBytes(4); //自己的QQ
            reader.ReadBytes(20);
            MessageDateTime = reader.ReadBytes(4);
            reader.BEReadChar(); //00
            reader.ReadBytes(4); //MessageDateTime
            reader.ReadBytes(5); //00
            reader.ReadBytes(3);
            reader.ReadBytes(5); //00
            reader.ReadBytes(4); //MessageDateTime
            reader.ReadBytes(4);
            reader.ReadBytes(8);
            FontStyle = reader.ReadBytes(reader.BEReadChar());
            reader.ReadBytes(6);
            MessageLength = reader.BEReadChar();
            MessageData = Util.ConvertHexToString(Util.ToHex(reader.ReadBytes(MessageLength)));
            reader.ReadBytes(22);
        }
    }
}