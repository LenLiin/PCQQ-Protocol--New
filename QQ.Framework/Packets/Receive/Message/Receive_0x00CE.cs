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
        public byte[] MessageDateTime{ get; set; }
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
        public Receive_0x00CE(ByteBuffer byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }
        protected override void ParseBody(ByteBuffer byteBuffer)
        {
            //密文
            byte[] CipherText = byteBuffer.ToByteArray();
            //明文
            bodyDecrypted = QQTea.Decrypt(CipherText, byteBuffer.Position, CipherText.Length - byteBuffer.Position - 1, user.QQ_SessionKey);
            //提取数据
            ByteBuffer buf = new ByteBuffer(bodyDecrypted);
            FromQQ = (long)Util.GetQQNumRetUint(Util.ToHex(buf.GetByteArray(4)));
            buf.GetByteArray(4);//自己的QQ
            buf.GetByteArray(10);
            MessageType = buf.GetByteArray(2);
            buf.GetChar();
            buf.GetByteArray(buf.GetChar());//未知
            buf.GetChar();//消息来源QQ的版本号
            buf.GetByteArray(4);//FromQQ
            buf.GetByteArray(4);//自己的QQ
            buf.GetByteArray(20);
            MessageDateTime = buf.GetByteArray(4);
            buf.GetChar();//00
            buf.GetByteArray(4);//MessageDateTime
            buf.GetByteArray(5);//00
            buf.GetByteArray(3);
            buf.GetByteArray(5);//00
            buf.GetByteArray(4);//MessageDateTime
            buf.GetByteArray(4);
            buf.GetByteArray(8);
            FontStyle = buf.GetByteArray(buf.GetChar());
            buf.GetByteArray(6);
            MessageLength = buf.GetChar();
            MessageData = Util.ConvertHexToString(Util.ToHex(buf.GetByteArray(MessageLength)));
            buf.GetByteArray(22);
        }
    }
}
