using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0x0017 : ReceivePacket
    {
        /// <summary>
        /// 消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }
        /// <summary>
        /// 群号
        /// </summary>
        public long Group { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public char MessageType { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public byte[] Font { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        public byte[] ReceiveTime { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public byte[] SendTime { get; set; }
        /// <summary>
        /// 群消息
        /// </summary>
        public Receive_0x0017(ByteBuffer byteBuffer, QQUser User)
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
            buf.GetByteArray(4);
            buf.GetByteArray(4);//自己的QQ
            buf.GetByteArray(10);
            MessageType = buf.GetChar();//消息类型
            if (MessageType == (char)0x0052)
            {
                buf.GetByteArray(2);
                buf.GetByteArray(buf.GetChar());
                Group = (long)Util.GetQQNumRetUint(Util.ToHex(buf.GetByteArray(4)));//群号
                buf.Get();
                FromQQ = (long)Util.GetQQNumRetUint(Util.ToHex(buf.GetByteArray(4)));//发消息人的QQ
                buf.GetByteArray(4);
                ReceiveTime = buf.GetByteArray(4);//接收时间  
                buf.GetByteArray(24);
                SendTime = buf.GetByteArray(4);//发送时间  
                buf.GetByteArray(12);
                Font = buf.GetByteArray(buf.GetChar());//字体
                buf.GetByteArray(6);
                Message = Util.ConvertHexToString(Util.ToHex(buf.GetByteArray(buf.GetChar())));//消息
                buf.GetByteArray(58);
                buf.GetByteArray(buf.GetChar());//消息
                buf.GetByteArray(11);
            }
        }
    }
}
