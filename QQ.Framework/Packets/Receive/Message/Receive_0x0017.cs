using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0x0017 : ReceivePacket
    {
        /// <summary>
        ///     群消息
        /// </summary>
        public Receive_0x0017(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        /// <summary>
        ///     消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }

        /// <summary>
        ///     群号
        /// </summary>
        public long Group { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public char MessageType { get; set; }

        /// <summary>
        ///     消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     字体
        /// </summary>
        public byte[] Font { get; set; }

        /// <summary>
        ///     接收时间
        /// </summary>
        public byte[] ReceiveTime { get; set; }

        /// <summary>
        ///     发送时间
        /// </summary>
        public byte[] SendTime { get; set; }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            reader.ReadBytes(4);
            reader.ReadBytes(4); //自己的QQ
            reader.ReadBytes(10);
            MessageType = reader.BEReadChar(); //消息类型
            if (MessageType == (char)0x0052)
            {
                reader.ReadBytes(2);
                reader.ReadBytes(reader.BEReadChar());
                Group = (long)Util.GetQQNumRetUint(Util.ToHex(reader.ReadBytes(4))); //群号
                reader.ReadByte();
                FromQQ = (long)Util.GetQQNumRetUint(Util.ToHex(reader.ReadBytes(4))); //发消息人的QQ
                reader.ReadBytes(4);
                ReceiveTime = reader.ReadBytes(4); //接收时间  
                reader.ReadBytes(24);
                SendTime = reader.ReadBytes(4); //发送时间  
                reader.ReadBytes(12);
                Font = reader.ReadBytes(reader.BEReadChar()); //字体
                reader.ReadBytes(6);
                Message = Encoding.UTF8.GetString(reader.ReadBytes(reader.BEReadChar())); //消息
                reader.ReadBytes(58);
                reader.ReadBytes(reader.BEReadChar()); //消息
                reader.ReadBytes(11);
            }
            else if (MessageType == (char)0x0058)
            {

            }
        }
    }
}